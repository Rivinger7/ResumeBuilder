namespace ResumeBuilder.Domain.Models.Resumes;

public sealed record ResumeSettingInternalResponse(int Column, string Font, float FontSize, float LineHeight, int SpaceBetweenElements, int LeftRightMargin, float SectionTitleFontSize, string SectionTitleColor, string BackgroundColorHeader, bool IsPagenNumbersEnabled);
