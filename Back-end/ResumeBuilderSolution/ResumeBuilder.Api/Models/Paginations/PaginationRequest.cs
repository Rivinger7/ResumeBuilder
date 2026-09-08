namespace ResumeBuilder.Api.Models.Paginations;

public sealed record PaginationRequest(int PageNumber = 1, int PageSize = 10);
