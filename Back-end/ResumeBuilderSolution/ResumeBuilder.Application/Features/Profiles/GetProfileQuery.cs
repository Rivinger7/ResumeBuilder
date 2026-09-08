using MediatR;

namespace ResumeBuilder.Application.Features.Profiles;

public sealed record GetProfileQuery(Guid UserId) : IRequest<GetProfileResponse>;