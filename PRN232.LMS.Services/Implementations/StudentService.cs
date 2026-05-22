using Microsoft.EntityFrameworkCore;
using System.Dynamic;
using PRN232.LMS.Repositories.Entities;
using PRN232.LMS.Repositories.Interfaces;
using PRN232.LMS.Services.BusinessModels.Common;
using PRN232.LMS.Services.BusinessModels.Students;
using PRN232.LMS.Services.Exceptions;
using PRN232.LMS.Services.Helpers;
using PRN232.LMS.Services.Interfaces;

namespace PRN232.LMS.Services.Implementations;

public class StudentService : IStudentService
{
    private readonly IStudentRepository _studentRepository;

    public StudentService(
        IStudentRepository studentRepository)
    {
        _studentRepository = studentRepository;
    }

    public async Task<
        PaginationResult<ExpandoObject>>
        GetStudentsAsync(
            string? search,
            string? sort,
            string? fields,
            string? expand,
            int page,
            int size)
    {
        var includeEnrollments =
            string.Equals(
                expand,
                "enrollments",
                StringComparison.OrdinalIgnoreCase);

        var query =
            _studentRepository
                .GetStudentsQueryable(
                    includeEnrollments);

        // SEARCH
        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(x =>
                x.FullName.Contains(search) ||
                x.Email.Contains(search));
        }

        // SORT
        if (!string.IsNullOrWhiteSpace(sort))
        {
            var sorts = sort.Split(',');

            foreach (var item in sorts)
            {
                var descending =
                    item.StartsWith("-");

                var field =
                    descending
                        ? item.Substring(1)
                        : item;

                query = field.ToLower() switch
                {
                    "fullname" =>
                        descending
                            ? query.OrderByDescending(
                                x => x.FullName)
                            : query.OrderBy(
                                x => x.FullName),

                    "dateofbirth" =>
                        descending
                            ? query.OrderByDescending(
                                x => x.DateOfBirth)
                            : query.OrderBy(
                                x => x.DateOfBirth),

                    _ => query
                };
            }
        }

        var totalItems =
            await query.CountAsync();

        var students =
            await query
                .Skip((page - 1) * size)
                .Take(size)
                .Select(x =>
                    new StudentExpandBusinessModel
                    {
                        StudentId = x.StudentId,
                        FullName = x.FullName,
                        Email = x.Email,
                        DateOfBirth = x.DateOfBirth,
                        Enrollments = includeEnrollments
                            ? x.Enrollments.Select(e => new
                            {
                                e.EnrollmentId,
                                e.CourseId,
                                CourseName =
                                    e.Course != null
                                        ? e.Course.CourseName
                                        : null,
                                e.Status
                            }).ToList()
                            : null
                    })
                .ToListAsync();

        var shapedStudents =
            students.ShapeData(fields);

        return new PaginationResult<
            ExpandoObject>
        {
            Items = shapedStudents,

            Pagination =
                new PaginationMetadata
                {
                    Page = page,
                    PageSize = size,
                    TotalItems = totalItems,

                    TotalPages =
                        (int)Math.Ceiling(
                            totalItems /
                            (double)size)
                }
        };
    }

    public async Task<StudentBusinessModel>
        GetStudentByIdAsync(int id)
    {
        var student =
            await _studentRepository.GetByIdAsync(id);

        if (student == null)
        {
            throw new NotFoundException(
                "Student not found");
        }

        return new StudentBusinessModel
        {
            StudentId = student.StudentId,
            FullName = student.FullName,
            Email = student.Email,
            DateOfBirth = student.DateOfBirth
        };
    }

    public async Task<StudentBusinessModel>
        CreateStudentAsync(
            StudentBusinessModel model)
    {
        var existingStudent =
            await _studentRepository
                .GetStudentByEmailAsync(
                    model.Email);

        if (existingStudent != null)
        {
            throw new ConflictException(
                "Email already exists");
        }

        var student = new Student
        {
            FullName = model.FullName,
            Email = model.Email,
            DateOfBirth = model.DateOfBirth
        };

        await _studentRepository.AddAsync(student);

        await _studentRepository.SaveChangesAsync();

        return new StudentBusinessModel
        {
            StudentId = student.StudentId,
            FullName = student.FullName,
            Email = student.Email,
            DateOfBirth = student.DateOfBirth
        };
    }

    public async Task<StudentBusinessModel>
        UpdateStudentAsync(
            int id,
            StudentBusinessModel model)
    {
        var student =
            await _studentRepository.GetByIdAsync(id);

        if (student == null)
        {
            throw new NotFoundException(
                "Student not found");
        }

        var existingStudent =
            await _studentRepository
                .GetStudentByEmailAsync(
                    model.Email);

        if (existingStudent != null
            && existingStudent.StudentId != id)
        {
            throw new ConflictException(
                "Email already exists");
        }

        student.FullName = model.FullName;
        student.Email = model.Email;
        student.DateOfBirth =
            model.DateOfBirth;

        _studentRepository.Update(student);

        await _studentRepository.SaveChangesAsync();

        return new StudentBusinessModel
        {
            StudentId = student.StudentId,
            FullName = student.FullName,
            Email = student.Email,
            DateOfBirth = student.DateOfBirth
        };
    }

    public async Task DeleteStudentAsync(
        int id)
    {
        var student =
            await _studentRepository.GetByIdAsync(id);

        if (student == null)
        {
            throw new NotFoundException(
                "Student not found");
        }

        _studentRepository.Delete(student);

        await _studentRepository.SaveChangesAsync();
    }
}