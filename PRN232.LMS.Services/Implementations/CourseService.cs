using System.Dynamic;
using Microsoft.EntityFrameworkCore;
using PRN232.LMS.Repositories.Entities;
using PRN232.LMS.Repositories.Interfaces;
using PRN232.LMS.Services.BusinessModels.Common;
using PRN232.LMS.Services.BusinessModels.Courses;
using PRN232.LMS.Services.Exceptions;
using PRN232.LMS.Services.Helpers;
using PRN232.LMS.Services.Interfaces;

namespace PRN232.LMS.Services.Implementations;

public class CourseService : ICourseService
{
    private readonly ICourseRepository _courseRepository;

    public CourseService(
        ICourseRepository courseRepository)
    {
        _courseRepository = courseRepository;
    }

    public async Task<
        PaginationResult<ExpandoObject>>
        GetCoursesAsync(
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

        var includeSemester =
            expandSet.Contains("semester");

        var includeSubject =
            expandSet.Contains("subject");

        var includeEnrollments =
            expandSet.Contains("enrollments");

        var query = _courseRepository.GetCoursesQueryable(
            includeSemester,
            includeSubject,
            includeEnrollments);

        // SEARCH
        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(x =>
                x.CourseName.Contains(search));
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
                    "coursename" =>
                        descending
                            ? query.OrderByDescending(
                                x => x.CourseName)
                            : query.OrderBy(
                                x => x.CourseName),

                    "semesterid" =>
                        descending
                            ? query.OrderByDescending(
                                x => x.SemesterId)
                            : query.OrderBy(
                                x => x.SemesterId),

                    "subjectid" =>
                        descending
                            ? query.OrderByDescending(
                                x => x.SubjectId)
                            : query.OrderBy(
                                x => x.SubjectId),

                    _ => query
                };
            }
        }

        var totalItems =
            await query.CountAsync();

        var courses = await query
            .Skip((page - 1) * size)
            .Take(size)
            .Select(x =>
                new CourseExpandBusinessModel
                {
                    CourseId = x.CourseId,
                    CourseName = x.CourseName,
                    SemesterId = x.SemesterId,
                    SubjectId = x.SubjectId,
                    Semester = includeSemester
                        ? new
                        {
                            x.Semester.SemesterId,
                            x.Semester.SemesterName,
                            x.Semester.StartDate,
                            x.Semester.EndDate
                        }
                        : null,
                    Subject = includeSubject
                        ? new
                        {
                            x.Subject.SubjectId,
                            x.Subject.SubjectCode,
                            x.Subject.SubjectName
                        }
                        : null,
                    Enrollments = includeEnrollments
                        ? x.Enrollments.Select(e => new
                        {
                            e.EnrollmentId,
                            e.StudentId,
                            e.CourseId,
                            e.EnrollDate,
                            e.Status
                        }).ToList()
                        : null
                })
            .ToListAsync();

        var shapedCourses =
            courses.ShapeData(fields);

        return new PaginationResult<
            ExpandoObject>
        {
            Items = shapedCourses,
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

    public async Task<CourseExpandBusinessModel>
        GetCourseByIdAsync(int id)
    {
        var course =
            await _courseRepository
                .GetCourseWithDetailsAsync(id);

        if (course == null)
        {
            throw new NotFoundException(
                "Course not found");
        }

        return new CourseExpandBusinessModel
        {
            CourseId = course.CourseId,
            CourseName = course.CourseName,
            SemesterId = course.SemesterId,
            SubjectId = course.SubjectId,
            Semester = new
            {
                course.Semester.SemesterId,
                course.Semester.SemesterName,
                course.Semester.StartDate,
                course.Semester.EndDate
            },
            Subject = new
            {
                course.Subject.SubjectId,
                course.Subject.SubjectCode,
                course.Subject.SubjectName
            },
            Enrollments = course.Enrollments.Select(e => new
            {
                e.EnrollmentId,
                e.StudentId,
                e.CourseId,
                e.EnrollDate,
                e.Status
            }).ToList()
        };
    }

    public async Task<CourseBusinessModel>
        CreateCourseAsync(
            CourseBusinessModel model)
    {
        var course = new Course
        {
            CourseName = model.CourseName,
            SemesterId = model.SemesterId,
            SubjectId = model.SubjectId
        };

        await _courseRepository.AddAsync(course);

        await _courseRepository.SaveChangesAsync();

        return new CourseBusinessModel
        {
            CourseId = course.CourseId,
            CourseName = course.CourseName,
            SemesterId = course.SemesterId,
            SubjectId = course.SubjectId
        };
    }

    public async Task<CourseBusinessModel>
        UpdateCourseAsync(
            int id,
            CourseBusinessModel model)
    {
        var course =
            await _courseRepository.GetByIdAsync(id);

        if (course == null)
        {
            throw new NotFoundException(
                "Course not found");
        }

        course.CourseName = model.CourseName;
        course.SemesterId = model.SemesterId;
        course.SubjectId = model.SubjectId;

        _courseRepository.Update(course);

        await _courseRepository.SaveChangesAsync();

        return new CourseBusinessModel
        {
            CourseId = course.CourseId,
            CourseName = course.CourseName,
            SemesterId = course.SemesterId,
            SubjectId = course.SubjectId
        };
    }

    public async Task DeleteCourseAsync(int id)
    {
        var course =
            await _courseRepository.GetByIdAsync(id);

        if (course == null)
        {
            throw new NotFoundException(
                "Course not found");
        }

        _courseRepository.Delete(course);

        await _courseRepository.SaveChangesAsync();
    }
}
