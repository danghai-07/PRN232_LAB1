using PRN232.LMS.Repositories.Entities;

namespace PRN232.LMS.Repositories.Interfaces;

public interface IEnrollmentRepository : IGenericRepository<Enrollment>
{
    IQueryable<Enrollment> GetEnrollmentsQueryable(
        bool includeCourse = false,
        bool includeStudent = false);

    Task<Enrollment?> GetEnrollmentWithDetailsAsync(int id);
}
