using System.Dynamic;
using Microsoft.EntityFrameworkCore;
using PRN232.LMS.Repositories.Entities;
using PRN232.LMS.Repositories.Interfaces;
using PRN232.LMS.Services.BusinessModels.Common;
using PRN232.LMS.Services.BusinessModels.Enrollments;
using PRN232.LMS.Services.Exceptions;
using PRN232.LMS.Services.Helpers;
using PRN232.LMS.Services.Interfaces;

namespace PRN232.LMS.Services.Implementations;

public class EnrollmentService : IEnrollmentService
{
    private readonly IEnrollmentRepository _enrollmentRepository;

    public EnrollmentService(
        IEnrollmentRepository enrollmentRepository)
    {
        _enrollmentRepository = enrollmentRepository;
    }

    public async Task<
        PaginationResult<ExpandoObject>>
        GetEnrollmentsAsync(
            string? search,
            string? sort,
            string? fields,
            string? expand,
            int page,
            int size)
    {
        var expandSet = string.IsNullOrWhiteSpace(expand)
            ? new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            : expand.Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(value => value.Trim())
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

        var includeCourse =
            expandSet.Contains("course");

        var includeStudent =
            expandSet.Contains("student");

        var query =
            _enrollmentRepository
                .GetEnrollmentsQueryable(
                    includeCourse,
                    includeStudent);

        // SEARCH
        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(x =>
                x.Status.Contains(search));
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
                    "enrolldate" =>
                        descending
                            ? query.OrderByDescending(
                                x => x.EnrollDate)
                            : query.OrderBy(
                                x => x.EnrollDate),

                    "status" =>
                        descending
                            ? query.OrderByDescending(
                                x => x.Status)
                            : query.OrderBy(
                                x => x.Status),

                    "studentid" =>
                        descending
                            ? query.OrderByDescending(
                                x => x.StudentId)
                            : query.OrderBy(
                                x => x.StudentId),

                    "courseid" =>
                        descending
                            ? query.OrderByDescending(
                                x => x.CourseId)
                            : query.OrderBy(
                                x => x.CourseId),

                    _ => query
                };
            }
        }

        var totalItems =
            await query.CountAsync();

        var enrollments =
            await query
                .Skip((page - 1) * size)
                .Take(size)
                .Select(x =>
                    new EnrollmentExpandBusinessModel
                    {
                        EnrollmentId = x.EnrollmentId,
                        StudentId = x.StudentId,
                        CourseId = x.CourseId,
                        EnrollDate = x.EnrollDate,
                        Status = x.Status,
                        Student = includeStudent
                            ? new
                            {
                                x.Student.StudentId,
                                x.Student.FullName,
                                x.Student.Email,
                                x.Student.DateOfBirth
                            }
                            : null,
                        Course = includeCourse
                            ? new
                            {
                                x.Course.CourseId,
                                x.Course.CourseName,
                                x.Course.SemesterId,
                                x.Course.SubjectId
                            }
                            : null
                    })
                .ToListAsync();

        var shapedEnrollments =
            enrollments.ShapeData(fields);

        return new PaginationResult<
            ExpandoObject>
        {
            Items = shapedEnrollments,
            Pagination = new PaginationMetadata
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

    public async Task<EnrollmentExpandBusinessModel>
        GetEnrollmentByIdAsync(int id)
    {
        var enrollment =
            await _enrollmentRepository
                .GetEnrollmentWithDetailsAsync(id);

        if (enrollment == null)
        {
            throw new NotFoundException(
                "Enrollment not found");
        }

        return new EnrollmentExpandBusinessModel
        {
            EnrollmentId = enrollment.EnrollmentId,
            StudentId = enrollment.StudentId,
            CourseId = enrollment.CourseId,
            EnrollDate = enrollment.EnrollDate,
            Status = enrollment.Status,
            Student = new
            {
                enrollment.Student.StudentId,
                enrollment.Student.FullName,
                enrollment.Student.Email,
                enrollment.Student.DateOfBirth
            },
            Course = new
            {
                enrollment.Course.CourseId,
                enrollment.Course.CourseName,
                enrollment.Course.SemesterId,
                enrollment.Course.SubjectId
            }
        };
    }

    public async Task<EnrollmentBusinessModel>
        CreateEnrollmentAsync(
            EnrollmentBusinessModel model)
    {
        var enrollment = new Enrollment
        {
            StudentId = model.StudentId,
            CourseId = model.CourseId,
            EnrollDate = model.EnrollDate,
            Status = model.Status
        };

        await _enrollmentRepository.AddAsync(enrollment);

        await _enrollmentRepository.SaveChangesAsync();

        return new EnrollmentBusinessModel
        {
            EnrollmentId = enrollment.EnrollmentId,
            StudentId = enrollment.StudentId,
            CourseId = enrollment.CourseId,
            EnrollDate = enrollment.EnrollDate,
            Status = enrollment.Status
        };
    }

    public async Task<EnrollmentBusinessModel>
        UpdateEnrollmentAsync(
            int id,
            EnrollmentBusinessModel model)
    {
        var enrollment =
            await _enrollmentRepository.GetByIdAsync(id);

        if (enrollment == null)
        {
            throw new NotFoundException(
                "Enrollment not found");
        }

        enrollment.StudentId = model.StudentId;
        enrollment.CourseId = model.CourseId;
        enrollment.EnrollDate = model.EnrollDate;
        enrollment.Status = model.Status;

        _enrollmentRepository.Update(enrollment);

        await _enrollmentRepository.SaveChangesAsync();

        return new EnrollmentBusinessModel
        {
            EnrollmentId = enrollment.EnrollmentId,
            StudentId = enrollment.StudentId,
            CourseId = enrollment.CourseId,
            EnrollDate = enrollment.EnrollDate,
            Status = enrollment.Status
        };
    }

    public async Task DeleteEnrollmentAsync(int id)
    {
        var enrollment =
            await _enrollmentRepository.GetByIdAsync(id);

        if (enrollment == null)
        {
            throw new NotFoundException(
                "Enrollment not found");
        }

        _enrollmentRepository.Delete(enrollment);

        await _enrollmentRepository.SaveChangesAsync();
    }
}
