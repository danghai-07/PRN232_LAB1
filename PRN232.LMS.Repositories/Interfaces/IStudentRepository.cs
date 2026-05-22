using PRN232.LMS.Repositories.Entities;

namespace PRN232.LMS.Repositories.Interfaces;

public interface IStudentRepository : IGenericRepository<Student>
{
    IQueryable<Student> Query();

    IQueryable<Student> GetStudentsQueryable(bool includeEnrollments = false);

    Task<Student?> GetStudentByEmailAsync(string email);
}