using ResumeBuilder.Domain.Common;

namespace ResumeBuilder.Domain.Entities.Resumes;

public sealed class ResumeSetting : BaseEntity
{
    public Guid ResumeId { get; set; }

    // Layout
    public int Column { get; set; }

    // Font & Font Size
    public string Font { get; set; } = null!;
    public float FontSize { get; set; } // Pt

    // Spacing
    public float LineHeight { get; set; } // Multiplier
    public int SpaceBetweenElements { get; set; } // Pt
    public int LeftRightMargin { get; set; } // Pt

    // Header
    public string BackgroundColorHeader { get; set; } = null!;

    // Section Title
    public float SectionTitleFontSize { get; set; }
    public string SectionTitleColor { get; set; } = null!;

    // Footer
    public bool IsPagenNumbersEnabled { get; set; }

    public Resume Resume { get; set; } = null!;
}
