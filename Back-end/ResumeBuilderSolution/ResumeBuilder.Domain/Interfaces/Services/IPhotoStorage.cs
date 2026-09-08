namespace ResumeBuilder.Domain.Interfaces.Services;

public interface IPhotoStorage
{
    Task<string> SaveAsync(Guid personalInformationEntryId, Stream content, string fileExtension, CancellationToken cancellationToken = default);
    Task<(byte[] Content, string ContentType)?> GetAsync(Guid personalInformationEntryId, CancellationToken cancellationToken = default);
}
