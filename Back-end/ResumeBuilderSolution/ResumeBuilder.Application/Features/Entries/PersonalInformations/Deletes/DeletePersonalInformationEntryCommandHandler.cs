using MediatR;

namespace ResumeBuilder.Application.Features.Entries.PersonalInformations.Deletes;

public sealed class DeletePersonalInformationEntryCommandHandler : IRequestHandler<DeletePersonalInformationEntryCommand, DeletePersonalInformationEntryResponse>
{
    public async Task<DeletePersonalInformationEntryResponse> Handle(DeletePersonalInformationEntryCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}