namespace PRN232.LMS.Services
    .BusinessModels.Common;

public class PaginationResult<T>
{
    public IEnumerable<T> Items { get; set; }
        = new List<T>();

    public PaginationMetadata Pagination
    { get; set; } = null!;
}