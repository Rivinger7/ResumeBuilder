namespace ResumeBuilder.Application.Features.Resumes.Pdfs.Exports;

public sealed record ExportResumePdfResponse(byte[] Content, string FileName);
