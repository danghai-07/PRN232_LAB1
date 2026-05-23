using PRN232.LMS.Repositories.Entities;

namespace PRN232.LMS.Repositories.Interfaces;

public interface ISubjectRepository : IGenericRepository<Subject>
{
    IQueryable<Subject> GetSubjectsQueryable(
        bool includeCourses = false);

    Task<Subject?> GetSubjectWithDetailsAsync(int id);
}
