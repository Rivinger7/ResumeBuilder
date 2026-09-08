using QuestPDF.Fluent;
using ResumeBuilder.Domain.Exceptions;
using ResumeBuilder.Domain.Interfaces.Services;
using ResumeBuilder.Domain.Models.Resumes;

namespace ResumeBuilder.Infrastructure.PDFs;

public sealed class ThumnailGenerator : IThumbnailGenerator
{
    private readonly string _storageRoot;

    public ThumnailGenerator(string webRootPath)
    {
        if (string.IsNullOrWhiteSpace(webRootPath))
        {
            throw new ArgumentException(
                "WebRootPath không được để trống.",
                nameof(webRootPath));
        }

        _storageRoot = Path.Combine(
            webRootPath,
            "uploads",
            "thumbnails");

        // Tạo thư mục nếu chưa tồn tại
        Directory.CreateDirectory(_storageRoot);
    }

    public async Task<string> GenerateAsync(
        ResumeInternalResponse resume,
        CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(resume);

        ResumeDocument document = new(resume);

        byte[][] images = document.GenerateImages().ToArray();

        if (images.Length == 0)
        {
            throw new BadRequestException(
                $"Không generate được thumbnail cho resume {resume.Id}");
        }

        string fileName = $"{resume.Id}.png";
        string filePath = Path.Combine(_storageRoot, fileName);

        await File.WriteAllBytesAsync(
            filePath,
            images[0],
            ct);

        // URL trỏ về API BE (không phải static file path) — FE ghép với origin của BE
        // (khác origin với FE dev server) khi bind vào <img src>.
        return $"/api/resumes/{resume.Id}/thumbnail";
    }

    public async Task<byte[]?> GetAsync(Guid resumeId, CancellationToken ct = default)
    {
        string filePath = Path.Combine(_storageRoot, $"{resumeId}.png");

        if (!File.Exists(filePath))
        {
            return null;
        }

        return await File.ReadAllBytesAsync(filePath, ct);
    }
}