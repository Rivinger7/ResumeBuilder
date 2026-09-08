using ResumeBuilder.Domain.Interfaces.Services;

namespace ResumeBuilder.Infrastructure.Storage;

public sealed class PhotoStorage : IPhotoStorage
{
    private readonly string _storageRoot;

    public PhotoStorage(string webRootPath)
    {
        if (string.IsNullOrWhiteSpace(webRootPath))
        {
            throw new ArgumentException("webRootPath must not be empty.", nameof(webRootPath));
        }

        _storageRoot = Path.Combine(webRootPath, "uploads", "photos");

        Directory.CreateDirectory(_storageRoot);
    }

    public async Task<string> SaveAsync(Guid personalInformationEntryId, Stream content, string fileExtension, CancellationToken cancellationToken = default)
    {
        // Only one photo per entry — remove any previous file (possibly a different extension)
        // before writing the new one.
        foreach (string existingFile in Directory.EnumerateFiles(_storageRoot, $"{personalInformationEntryId}.*"))
        {
            File.Delete(existingFile);
        }

        string filePath = Path.Combine(_storageRoot, $"{personalInformationEntryId}{fileExtension}");

        await using FileStream fileStream = File.Create(filePath);
        await content.CopyToAsync(fileStream, cancellationToken);

        // URL points back at the API (not a static file path) — FE resolves it against
        // the BE origin, matching how resume thumbnails are served.
        return $"/api/entries/personal-information/{personalInformationEntryId}/photo";
    }

    public async Task<(byte[] Content, string ContentType)?> GetAsync(Guid personalInformationEntryId, CancellationToken cancellationToken = default)
    {
        string[] matches = Directory.GetFiles(_storageRoot, $"{personalInformationEntryId}.*");
        if (matches.Length == 0)
        {
            return null;
        }

        string filePath = matches[0];
        byte[] content = await File.ReadAllBytesAsync(filePath, cancellationToken);
        string contentType = Path.GetExtension(filePath).ToLowerInvariant() switch
        {
            ".png" => "image/png",
            ".jpg" or ".jpeg" => "image/jpeg",
            ".webp" => "image/webp",
            _ => "application/octet-stream",
        };

        return (content, contentType);
    }
}
