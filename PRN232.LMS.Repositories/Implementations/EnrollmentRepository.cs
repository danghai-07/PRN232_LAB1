using Microsoft.EntityFrameworkCore;
using PRN232.LMS.Repositories.Data;
using PRN232.LMS.Repositories.Entities;
using PRN232.LMS.Repositories.Interfaces;

namespace PRN232.LMS.Repositories.Implementations;

public class EnrollmentRepository
    : GenericRepository<Enrollment>,
      IEnrollmentRepository
{
    public EnrollmentRepository(AppDbContext context)
        : base(context)
    {
    }

    public IQueryable<Enrollment> GetEnrollmentsQueryable(
        bool includeCourse = false,
        bool includeStudent = false)
    {
        var query = _dbSet.AsQueryable();

        if (includeCourse)
        {
            query = query.Include(x => x.Course);
        }

        if (includeStudent)
        {
            query = query.Include(x => x.Student);
        }

        return query;
    }

    public async Task<Enrollment?> GetEnrollmentWithDetailsAsync(int id)
    {
        return await _dbSet
            .Include(x => x.Course)
            .Include(x => x.Student)
            .FirstOrDefaultAsync(x => x.EnrollmentId == id);
    }
}
