using PRN232.LMS.Repositories.Entities;

namespace PRN232.LMS.Repositories.Interfaces;

public interface ISemesterRepository : IGenericRepository<Semester>
{
    IQueryable<Semester> GetSemestersQueryable(
        bool includeCourses = false);

    Task<Semester?> GetSemesterWithDetailsAsync(int id);
}
