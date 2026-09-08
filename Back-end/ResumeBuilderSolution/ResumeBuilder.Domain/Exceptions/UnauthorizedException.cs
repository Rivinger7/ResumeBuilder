namespace ResumeBuilder.Domain.Exceptions;

public sealed class UnauthorizedException(string message) : BaseException(message);
