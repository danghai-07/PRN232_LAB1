using System.Dynamic;
using PRN232.LMS.Services
    .BusinessModels.Common;
using PRN232.LMS.Services
    .BusinessModels.Semesters;

namespace PRN232.LMS.Services.Interfaces;

public interface ISemesterService
{
    Task<
        PaginationResult<ExpandoObject>>
        GetSemestersAsync(
            string? search,
            string? sort,
            string? fields,
            string? expand,
            int page,
            int size);

    Task<SemesterExpandBusinessModel>
        GetSemesterByIdAsync(int id);

    Task<SemesterBusinessModel>
        CreateSemesterAsync(
            SemesterBusinessModel model);

    Task<SemesterBusinessModel>
        UpdateSemesterAsync(
            int id,
            SemesterBusinessModel model);

    Task DeleteSemesterAsync(int id);
}
