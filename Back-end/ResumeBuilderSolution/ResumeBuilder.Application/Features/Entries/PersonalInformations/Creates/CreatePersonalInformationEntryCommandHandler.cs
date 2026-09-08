using MediatR;

namespace ResumeBuilder.Application.Features.Entries.PersonalInformations.Creates;

public sealed class CreatePersonalInformationEntryCommandHandler : IRequestHandler<CreatePersonalInformationEntryCommand, CreatePersonalInformationEntryResponse>
{
    public async Task<CreatePersonalInformationEntryResponse> Handle(CreatePersonalInformationEntryCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}