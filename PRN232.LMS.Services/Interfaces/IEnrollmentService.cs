using System.Dynamic;
using PRN232.LMS.Services
    .BusinessModels.Common;
using PRN232.LMS.Services
    .BusinessModels.Enrollments;

namespace PRN232.LMS.Services.Interfaces;

public interface IEnrollmentService
{
    Task<
        PaginationResult<ExpandoObject>>
        GetEnrollmentsAsync(
            string? search,
            string? sort,
            string? fields,
            string? expand,
            int page,
            int size);

    Task<EnrollmentExpandBusinessModel>
        GetEnrollmentByIdAsync(int id);

    Task<EnrollmentBusinessModel>
        CreateEnrollmentAsync(
            EnrollmentBusinessModel model);

    Task<EnrollmentBusinessModel>
        UpdateEnrollmentAsync(
            int id,
            EnrollmentBusinessModel model);

    Task DeleteEnrollmentAsync(int id);
}
