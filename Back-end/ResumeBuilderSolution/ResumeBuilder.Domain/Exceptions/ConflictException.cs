namespace ResumeBuilder.Domain.Exceptions;

public sealed class ConflictException(string message) : BaseException(message);
