using Microsoft.EntityFrameworkCore;
using PRN232.LMS.Repositories.Data;
using PRN232.LMS.Repositories.Entities;
using PRN232.LMS.Repositories.Interfaces;

namespace PRN232.LMS.Repositories.Implementations;

public class SemesterRepository
    : GenericRepository<Semester>,
      ISemesterRepository
{
    public SemesterRepository(AppDbContext context)
        : base(context)
    {
    }

    public IQueryable<Semester> GetSemestersQueryable(
        bool includeCourses = false)
    {
        var query = _dbSet.AsQueryable();

        if (includeCourses)
        {
            query = query.Include(x => x.Courses);
        }

        return query;
    }

    public async Task<Semester?> GetSemesterWithDetailsAsync(int id)
    {
        return await _dbSet
            .Include(x => x.Courses)
            .FirstOrDefaultAsync(x => x.SemesterId == id);
    }
}
