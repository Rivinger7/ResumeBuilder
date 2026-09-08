using ResumeBuilder.Domain.Common;
using ResumeBuilder.Domain.Enums;

namespace ResumeBuilder.Domain.Entities.Resumes.Entries;

public sealed class PersonalInformationEntry : BaseEntity
{
    public Guid ResumeSectionId { get; set; }
    public string? FullName { get; set; }
    public string? ProfessionalTitle { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Address { get; set; }
    public string? Website { get; set; }
    public string? LinkedIn { get; set; }
    public string? GitHub { get; set; }
    public string? PhotoUrl { get; set; }
    public TextAlignmentType PersonalInformationTextAlignment { get; set; }
    public LayoutType PersonalInformationLayout { get; set; }
    public MarkerStyleType PersonalInformationMarkerStyle { get; set; }
    public IconStyleType PersonalInformationIconStyle { get; set; }
    public bool IsTitleProfessionalTitleOnSameLine { get; set; }

    public ResumeSection ResumeSection { get; set; } = null!;
}
