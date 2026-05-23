using Microsoft.EntityFrameworkCore;
using PRN232.LMS.Repositories.Data;
using PRN232.LMS.Repositories.Entities;
using PRN232.LMS.Repositories.Interfaces;

namespace PRN232.LMS.Repositories.Implementations;

public class StudentRepository
    : GenericRepository<Student>,
      IStudentRepository
{
    public StudentRepository(AppDbContext context)
       : base(context)
    {
    }

    public IQueryable<Student> Query()
    {
        return _dbSet.AsQueryable();
    }

    public IQueryable<Student> GetStudentsQueryable(bool includeEnrollments = false)
    {
        var query = _dbSet.AsQueryable();

        if (includeEnrollments)
        {
            query = query
                .Include(x => x.Enrollments)
                .ThenInclude(x => x.Course);
        }

        return query;
    }

    public async Task<Student?> GetStudentByEmailAsync(string email)
    {
        return await _dbSet.FirstOrDefaultAsync(x => x.Email == email);
    }
}