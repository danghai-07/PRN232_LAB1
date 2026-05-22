using PRN232.LMS.Services
    .BusinessModels.Common;
using PRN232.LMS.Services.BusinessModels.Students;
using System.Dynamic;
using PRN232.LMS.Services
    .BusinessModels.Students;

namespace PRN232.LMS.Services.Interfaces;

public interface IStudentService
{
    Task<
        PaginationResult<ExpandoObject>>
        GetStudentsAsync(
            string? search,
            string? sort,
            string? fields,
            string? expand,
            int page,
            int size);

    Task<StudentBusinessModel>
        GetStudentByIdAsync(int id);

    Task<StudentBusinessModel>
        CreateStudentAsync(
            StudentBusinessModel model);

    Task<StudentBusinessModel>
        UpdateStudentAsync(
            int id,
            StudentBusinessModel model);

    Task DeleteStudentAsync(int id);
}