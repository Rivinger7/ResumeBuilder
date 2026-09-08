namespace ResumeBuilder.Domain.Interfaces.Persistence;

public interface IDisplayOrderedEntry
{
    Guid Id { get; set; }
    Guid ResumeSectionId { get; set; }
    int DisplayOrder { get; set; }
}
