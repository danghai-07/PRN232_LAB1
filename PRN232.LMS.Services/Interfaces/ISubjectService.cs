using System.Dynamic;
using PRN232.LMS.Services
    .BusinessModels.Common;
using PRN232.LMS.Services
    .BusinessModels.Subjects;

namespace PRN232.LMS.Services.Interfaces;

public interface ISubjectService
{
    Task<
        PaginationResult<ExpandoObject>>
        GetSubjectsAsync(
            string? search,
            string? sort,
            string? fields,
            string? expand,
            int page,
            int size);

    Task<SubjectExpandBusinessModel>
        GetSubjectByIdAsync(int id);

    Task<SubjectBusinessModel>
        CreateSubjectAsync(
            SubjectBusinessModel model);

    Task<SubjectBusinessModel>
        UpdateSubjectAsync(
            int id,
            SubjectBusinessModel model);

    Task DeleteSubjectAsync(int id);
}
