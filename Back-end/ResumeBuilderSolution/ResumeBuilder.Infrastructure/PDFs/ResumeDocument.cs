using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using ResumeBuilder.Domain.Enums;
using ResumeBuilder.Domain.Models.Resumes;
using ResumeBuilder.Domain.Models.Resumes.Entries;
using ResumeBuilder.Infrastructure.PDFs.Helpers;
using System.Data;

namespace ResumeBuilder.Infrastructure.PDFs;

/// <summary>
/// Build layout PDF từ ResumeInternalResponse bằng QuestPDF, theo mẫu thiết kế:
/// header navy full-width + tên/chức danh/contact info trắng căn giữa,
/// section title có gạch chân đậm, spacing rõ ràng.
///
/// Font/FontSize/LineHeight/margin lấy từ ResumeSettingInternalResponse — không hard-code.
/// Màu header hiện fix cứng (NavyColor) — Settings chưa có field theme/color,
/// khi có thì thay hằng số này bằng giá trị tương ứng.
/// </summary>
public sealed class ResumeDocument(ResumeInternalResponse resumeInternalResponse) : IDocument
{
    private readonly ResumeInternalResponse _resume = resumeInternalResponse;
    private readonly ResumeSettingInternalResponse _settings = resumeInternalResponse.Settings;

    // Important CSS Value Convertion
    // Pixel -> Pointer: X * 0.75 = Y (Pointer)

    // TODO: thay bằng field theme/color khi Settings có
    private static readonly Color DarkModerateBlueColor = Color.FromHex("#355C7D");
    private static readonly Color MutedColor = Colors.Grey.Darken1;
    private static readonly Color WhiteOpacity90 = Color.FromHex("#fff");
    private static readonly Color DividerColor = Color.FromHex("#202020");

    public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

    public void Compose(IDocumentContainer container)
    {
        container.Page(page =>
        {
            page.Size(PageSizes.A4);
            page.Margin(0);
            page.DefaultTextStyle(x => x
                .FontFamily(_settings.Font)
                .FontSize(_settings.FontSize)
                .FontColor(Colors.Black)
                .LineHeight(_settings.LineHeight));

            page.Content().Column(column =>
            {
                column.Item().Element(ComposeHeader);

                column.Item()
                    .PaddingHorizontal(_settings.LeftRightMargin)
                    .PaddingVertical(20)
                    .Column(body =>
                    {
                        body.Spacing(_settings.SpaceBetweenElements);

                        foreach (ResumeSectionInternalResponse? section in (_resume.ResumeSections ?? [])
                            .Where(s => s.Type != ResumeSectionType.PersonalInformation)
                            .OrderBy(GetSectionOrder))
                        {
                            ComposeSection(body, section);
                        }
                    });

                if (_settings.IsPagenNumbersEnabled)
                {
                    column.Item().AlignCenter().PaddingBottom(10).Text(text =>
                    {
                        text.CurrentPageNumber().FontSize(8).FontColor(MutedColor);
                        text.Span(" / ").FontSize(8).FontColor(MutedColor);
                        text.TotalPages().FontSize(8).FontColor(MutedColor);
                    });
                }
            });
        });
    }

    private void ComposeHeader(IContainer container)
    {
        PersonalInformationEntryInternalResponse? personalInfo = _resume.ResumeSections?
            .FirstOrDefault(s => s.Type == ResumeSectionType.PersonalInformation)?
            .PersonalInformationEntries?.FirstOrDefault();

        container.Background(DarkModerateBlueColor).Padding(24).Column(header =>
        {
            header.Spacing(4);

            header.Item().AlignCenter().Text(personalInfo?.FullName ?? _resume.Title)
                .FontSize(_settings.FontSize * 2F).ApplyFontWeight(700).FontColor(Colors.White);

            if (!string.IsNullOrWhiteSpace(personalInfo?.ProfessionalTitle))
            {
                header.Item().AlignCenter().Text(personalInfo.ProfessionalTitle)
                    .FontSize(_settings.FontSize * 1.45F).Italic().FontColor(Colors.White);
            }

            if (personalInfo is not null)
            {
                List<(string IconKey, string Value)> contacts = new[]
                {
                    ("mail", personalInfo.Email),
                    ("phone", personalInfo.PhoneNumber),
                    ("map-pin", personalInfo.Address),
                    ("globe", personalInfo.Website),
                    ("linkedin", personalInfo.Linkedin),
                    ("github", personalInfo.Github),
                }
                .Where(x => !string.IsNullOrWhiteSpace(x.Item2))
                .Select(x => (x.Item1, x.Item2!))
                .ToList();

                MarkerStyleType markerStyle = ParseEnum(personalInfo.PersonalInformationMarkerStyle, MarkerStyleType.None);
                IconStyleType iconStyle = ParseEnum(personalInfo.PersonalInformationIconStyle, IconStyleType.None);

                bool showIcon = markerStyle == MarkerStyleType.Icon && iconStyle != IconStyleType.None;
                string separator = showIcon ? "" : markerStyle switch
                {
                    MarkerStyleType.Bar => "|",
                    MarkerStyleType.Bullet => "•",
                    _ => "",
                };

                // Mirrors resume-preview.ts: gap-1 (4px) between icon/value/marker inside a
                // contact, gap-2 (8px) between contacts — converted to pt (px * 0.75).
                const float withinContactGap = 3f;
                const float betweenContactsGap = 6f;

                header.Item()
                    .PaddingTop(6)
                    .AlignCenter()
                    .Inlined(inlined =>
                    {
                        inlined.HorizontalSpacing(0);
                        inlined.VerticalSpacing(betweenContactsGap);
                        inlined.BaselineMiddle();

                        for (int i = 0; i < contacts.Count; i++)
                        {
                            if (i > 0)
                            {
                                inlined.Item().Width(betweenContactsGap);
                            }

                            ComposeContactItem(inlined, contacts[i].IconKey, contacts[i].Value, showIcon, iconStyle, isLast: i == contacts.Count - 1, separator, withinContactGap);
                        }
                    });
            }
        });
    }

    private void ComposeContactItem(InlinedDescriptor inlined, string iconKey, string value, bool showIcon, IconStyleType iconStyle, bool isLast, string separator, float withinContactGap)
    {
        if (showIcon && ContactIconLibrary.Get(iconKey) is string svg)
        {
            // Default style mirrors FE's bare size-[1em] icon (no padding/background);
            // the other styles add FE's "p-1" (4px ≈ 3pt) padding around the icon.
            float iconPadding = iconStyle == IconStyleType.Default ? 0f : withinContactGap;
            float boxSize = _settings.FontSize + (iconPadding * 2);
            IContainer iconContainer = inlined.Item().Width(boxSize).Height(boxSize);

            ApplyIconStyleBackground(iconContainer, iconStyle)
                .Padding(iconPadding)
                .Svg(svg)
                .FitHeight();

            inlined.Item().Width(withinContactGap);
        }

        inlined.Item().Text(value).FontSize(_settings.FontSize).FontColor(WhiteOpacity90);

        if (!isLast && separator != "")
        {
            inlined.Item().Width(withinContactGap);
            inlined.Item().Text(separator).FontSize(_settings.FontSize).FontColor(WhiteOpacity90);
        }
    }

    private static IContainer ApplyIconStyleBackground(IContainer container, IconStyleType iconStyle) =>
        iconStyle switch
        {
            IconStyleType.CircleFilled => container.Background(Colors.White.WithAlpha(0.2f)).CornerRadius(999),
            IconStyleType.RoundedFilled => container.Background(Colors.White.WithAlpha(0.2f)).CornerRadius(4),
            IconStyleType.SquareFilled => container.Background(Colors.White.WithAlpha(0.2f)),
            IconStyleType.CircleOutline => container.Border(1).BorderColor(Colors.White.WithAlpha(0.5f)).CornerRadius(999),
            IconStyleType.RoundedOutline => container.Border(1).BorderColor(Colors.White.WithAlpha(0.5f)).CornerRadius(4),
            IconStyleType.SquareOutline => container.Border(1).BorderColor(Colors.White.WithAlpha(0.5f)),
            _ => container,
        };

    private void ComposeSection(ColumnDescriptor column, ResumeSectionInternalResponse section)
    {
        column.Item().Column(sectionColumn =>
        {
            sectionColumn.Spacing(4);

            sectionColumn.Item().Text(section.Title.ToUpperInvariant())
                .FontSize(_settings.FontSize).Bold().FontColor(_settings.SectionTitleColor);
            sectionColumn.Item().OffsetY(-3F).LineHorizontal(1.5F).LineColor(DividerColor);

            sectionColumn.Item().PaddingTop(2).Element(body => ComposeSectionBody(body, section));
        });
    }

    private void ComposeSectionBody(IContainer container, ResumeSectionInternalResponse section)
    {
        container.PaddingBottom(16).Column(body =>
        {
            body.Spacing(8);

            switch (section.Type)
            {
                case ResumeSectionType.Summary:
                    foreach (SummaryEntryInternalResponse e in section.SummaryEntries ?? [])
                    {
                        HtmlRichTextRenderer.Render(body, e.Summary);
                    }

                    break;

                case ResumeSectionType.Objective:
                    foreach (ObjectiveEntryInternalResponse e in section.ObjectiveEntries ?? [])
                    {
                        if (!string.IsNullOrWhiteSpace(e.Title))
                        {
                            body.Item().Text(e.Title).Bold();
                        }

                        HtmlRichTextRenderer.Render(body, e.Description);
                    }
                    break;

                case ResumeSectionType.Education:
                    foreach (EducationEntryInternalResponse e in
                        (section.EducationEntries ?? []).OrderBy(x => x.DisplayOrder))
                    {
                        body.Item().Row(row =>
                        {
                            row.RelativeItem().Text(e.SchoolName).Bold().FontSize(_settings.FontSize + 0.5f);
                            row.AutoItem().Text(FormatDateRange(e.StartDate, e.EndDate, e.IsCurrent))
                                .FontSize(_settings.FontSize - 1).FontColor(MutedColor);

                            if (!string.IsNullOrWhiteSpace(e.Location))
                            {
                                row.AutoItem().Text("|").FontSize(_settings.FontSize - 1).FontColor(MutedColor);
                                row.AutoItem().Text(e.Location).FontSize(_settings.FontSize - 1).FontColor(MutedColor);
                            }
                        });

                        if (!string.IsNullOrWhiteSpace(e.Degree))
                        {
                            body.Item().Text(e.Degree);
                        }

                        if (e.Major is not null)
                        {
                            body.Item().Text($"Major: {e.Major}");
                        }

                        if (e.GPA is not null)
                        {
                            body.Item().Text($"GPA: {e.GPA:0.00}/4");
                        }

                        HtmlRichTextRenderer.Render(body, e.Description);
                    }
                    break;

                case ResumeSectionType.Experience:
                    foreach (ExperienceEntryInternalResponse e in
                        (section.ExperienceEntries ?? []).OrderBy(x => x.DisplayOrder))
                    {
                        body.Item().Row(row =>
                        {
                            row.RelativeItem().Text($"{e.Position} — {e.CompanyName}")
                                .Bold().FontSize(_settings.FontSize + 0.5f);
                            row.AutoItem().Text(FormatDateRange(e.StartDate, e.EndDate, e.IsCurrent))
                                .FontSize(_settings.FontSize - 1).FontColor(MutedColor);
                        });

                        HtmlRichTextRenderer.Render(body, e.Description);
                    }
                    break;

                case ResumeSectionType.Projects:
                    foreach (ProjectEntryInternalResponse e in
                        (section.ProjectEntries ?? []).OrderBy(x => x.DisplayOrder))
                    {
                        body.Item().Row(row =>
                        {
                            row.RelativeItem().Text(e.Title).Bold().FontSize(_settings.FontSize + 0.5f);
                            row.AutoItem().Text(FormatDateRange(e.StartDate, e.EndDate, false))
                                .FontSize(_settings.FontSize - 1).FontColor(MutedColor);
                        });
                        if (!string.IsNullOrWhiteSpace(e.SubTitle))
                        {
                            body.Item().Text(e.SubTitle).Italic();
                        }

                        HtmlRichTextRenderer.Render(body, e.Description);
                    }
                    break;

                case ResumeSectionType.Certificates:
                    foreach (CertificateEntryInternalResponse e in
                        (section.CertificateEntries ?? []).OrderBy(x => x.DisplayOrder))
                    {
                        body.Item().Text(e.Title);
                    }
                    break;

                case ResumeSectionType.Languages:
                    foreach (LanguageEntryInternalResponse e in
                        (section.LanguageEntries ?? []).OrderBy(x => x.DisplayOrder))
                    {
                        body.Item().Text($"{e.LanguageName} ({e.Proficiency})");
                    }
                    break;

                case ResumeSectionType.Skills:
                    foreach (SkillEntryInternalResponse e in
                        (section.SkillEntries ?? []).OrderBy(x => x.DisplayOrder))
                    {
                        string skillLine = string.IsNullOrWhiteSpace(e.SkillLevel)
                            ? e.SkillName ?? ""
                            : $"{e.SkillName} ({e.SkillLevel})";
                        body.Item().Text(skillLine);
                    }
                    break;

                // Interests, Awards, Organizations, Publications, References, Declaration,
                // Achievements, Courses, Custom: chưa có entry type riêng.
                default:
                    break;
            }


        });
    }

    private static string FormatDateRange(DateOnly? start, DateOnly? end, bool isCurrent)
    {
        string startText = start?.ToString("MM/yyyy") ?? "";
        string endText = isCurrent ? "Present" : (end?.ToString("MM/yyyy") ?? "");
        return string.IsNullOrEmpty(startText) && string.IsNullOrEmpty(endText)
            ? ""
            : $"{startText} – {endText}";
    }

    private static TEnum ParseEnum<TEnum>(string? value, TEnum fallback) where TEnum : struct, Enum =>
        Enum.TryParse(value, out TEnum result) ? result : fallback;

    private static string JoinNonEmpty(string separator, params string?[] parts) =>
        string.Join(separator, parts.Where(p => !string.IsNullOrWhiteSpace(p)));

    private static int GetSectionOrder(ResumeSectionInternalResponse section) => section.DisplayOrder;
}