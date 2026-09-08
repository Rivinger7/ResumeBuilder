using MediatR;
using ResumeBuilder.Domain.Exceptions;
using ResumeBuilder.Domain.Interfaces.Repositories;
using ResumeBuilder.Domain.Models.Profiles;

namespace ResumeBuilder.Application.Features.Profiles;

public sealed class GetProfileQueryHandler(IUserRepository userRepository) : IRequestHandler<GetProfileQuery, GetProfileResponse>
{
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<GetProfileResponse> Handle(GetProfileQuery request, CancellationToken cancellationToken)
    {
        ProfileInternalResponse profileInternalResponse = await _userRepository.GetProfileByIdAsync(request.UserId, cancellationToken) ?? throw new NotFoundException("Not found profile");

        return new GetProfileResponse(profileInternalResponse.Email, profileInternalResponse.FullName, profileInternalResponse.AvatarUrl);
    }
}