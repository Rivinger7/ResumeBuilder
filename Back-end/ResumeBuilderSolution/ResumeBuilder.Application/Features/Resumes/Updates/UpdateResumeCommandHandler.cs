using MediatR;

namespace ResumeBuilder.Application.Features.Resumes.Updates;

public sealed class UpdateResumeCommandHandler : IRequestHandler<UpdateResumeCommand, UpdateResumeResponse>
{
    public async Task<UpdateResumeResponse> Handle(UpdateResumeCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}