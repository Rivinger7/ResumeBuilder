namespace ResumeBuilder.Domain.Interfaces.Persistence;

public interface IDisplayOrderedResumeSection
{
    Guid Id { get; set; }
    Guid ResumeId { get; set; }
    int DisplayOrder { get; set; }
}
