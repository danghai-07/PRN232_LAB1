using PRN232.LMS.Repositories.Entities;

namespace PRN232.LMS.Repositories.Interfaces;

public interface ICourseRepository : IGenericRepository<Course>
{
    IQueryable<Course> GetCoursesQueryable(
        bool includeSemester = false,
        bool includeSubject = false,
        bool includeEnrollments = false);

    Task<Course?> GetCourseWithDetailsAsync(int id);
}
