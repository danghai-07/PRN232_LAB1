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

    public async Task<Student?> GetStudentByEmailAsync(string email)
    {
        return await _dbSet.FirstOrDefaultAsync(x => x.Email == email);
    }
}