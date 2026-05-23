using Microsoft.EntityFrameworkCore;
using PRN232.LMS.Repositories.Data;
using PRN232.LMS.Repositories.Entities;
using PRN232.LMS.Repositories.Interfaces;

namespace PRN232.LMS.Repositories.Implementations;

public class CourseRepository
    : GenericRepository<Course>,
      ICourseRepository
{
    public CourseRepository(AppDbContext context)
        : base(context)
    {
    }

    public IQueryable<Course> GetCoursesQueryable(
        bool includeSemester = false,
        bool includeSubject = false,
        bool includeEnrollments = false)
    {
        var query = _dbSet.AsQueryable();

        if (includeSemester)
        {
            query = query.Include(x => x.Semester);
        }

        if (includeSubject)
        {
            query = query.Include(x => x.Subject);
        }

        if (includeEnrollments)
        {
            query = query.Include(x => x.Enrollments);
        }

        return query;
    }

    public async Task<Course?> GetCourseWithDetailsAsync(int id)
    {
        return await _dbSet
            .Include(x => x.Semester)
            .Include(x => x.Subject)
            .Include(x => x.Enrollments)
            .FirstOrDefaultAsync(x => x.CourseId == id);
    }
}
