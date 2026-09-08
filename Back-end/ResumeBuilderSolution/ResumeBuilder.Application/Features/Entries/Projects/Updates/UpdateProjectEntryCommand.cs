using MediatR;
using ResumeBuilder.Domain.Common;

namespace ResumeBuilder.Application.Features.Entries.Projects.Updates;

public sealed record UpdateProjectEntryCommand(Guid Id, Optional<string?> Title, Optional<string?> SubTitle, Optional<DateOnly?> StartDate, Optional<DateOnly?> EndDate, Optional<string?> ProjectUrl, Optional<string?> RepositoryUrl, Optional<string?> Description) : IRequest<UpdateProjectEntryResponse>;
