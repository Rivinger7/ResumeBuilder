namespace ResumeBuilder.Application.Models;

public sealed record PagedResponse<T>(IReadOnlyCollection<T> Items, int PageNumber, int PageSize, int TotalItems, int TotalPages, bool HasPreviousPage, bool HasNextPage)
{
    public static PagedResponse<T> Create(IReadOnlyCollection<T> items, int pageNumber, int pageSize, int totalItems)
    {
        int totalPages = totalItems == 0 ? 0 : (int)Math.Ceiling(totalItems / (double)pageSize);

        return new PagedResponse<T>(Items: items, PageNumber: pageNumber, PageSize: pageSize, TotalItems: totalItems, TotalPages: totalPages, HasPreviousPage: pageNumber > 1, HasNextPage: pageNumber < totalPages);
    }
}