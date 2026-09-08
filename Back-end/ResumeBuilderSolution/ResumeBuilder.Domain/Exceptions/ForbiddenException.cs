namespace ResumeBuilder.Domain.Exceptions;

public sealed class ForbiddenException(string message) : BaseException(message);
