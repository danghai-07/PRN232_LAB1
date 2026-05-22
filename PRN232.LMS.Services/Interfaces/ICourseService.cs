using System.Dynamic;
using PRN232.LMS.Services
    .BusinessModels.Common;
using PRN232.LMS.Services.BusinessModels.Courses;

namespace PRN232.LMS.Services.Interfaces;

public interface ICourseService
{
    Task<
        PaginationResult<ExpandoObject>>
        GetCoursesAsync(
            string? search,
            string? sort,
            string? fields,
            string? expand,
            int page,
            int size);

    Task<CourseExpandBusinessModel>
        GetCourseByIdAsync(int id);

    Task<CourseBusinessModel>
        CreateCourseAsync(
            CourseBusinessModel model);

    Task<CourseBusinessModel>
        UpdateCourseAsync(
            int id,
            CourseBusinessModel model);

    Task DeleteCourseAsync(int id);
}
