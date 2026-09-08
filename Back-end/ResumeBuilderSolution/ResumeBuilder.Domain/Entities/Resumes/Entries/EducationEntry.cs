using ResumeBuilder.Domain.Common;
using ResumeBuilder.Domain.Interfaces.Persistence;

namespace ResumeBuilder.Domain.Entities.Resumes.Entries;

public sealed class EducationEntry : BaseEntity, IDisplayOrderedEntry
{
    public Guid ResumeSectionId { get; set; }
    public string? SchoolName { get; set; }
    public string? Degree { get; set; }
    public string? Major { get; set; }
    public decimal? GPA { get; set; }
    public bool IsCurrent { get; set; }
    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public string? Location { get; set; }
    public string? Description { get; set; }
    public int DisplayOrder { get; set; }

    public bool IsByOrder { get; set; } // DegreeSchool, SchoolDegree

    public ResumeSection ResumeSection { get; set; } = null!;
}
