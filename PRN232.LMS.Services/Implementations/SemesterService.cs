using System.Dynamic;
using Microsoft.EntityFrameworkCore;
using PRN232.LMS.Repositories.Entities;
using PRN232.LMS.Repositories.Interfaces;
using PRN232.LMS.Services.BusinessModels.Common;
using PRN232.LMS.Services.BusinessModels.Semesters;
using PRN232.LMS.Services.Exceptions;
using PRN232.LMS.Services.Helpers;
using PRN232.LMS.Services.Interfaces;

namespace PRN232.LMS.Services.Implementations;

public class SemesterService : ISemesterService
{
    private readonly ISemesterRepository _semesterRepository;

    public SemesterService(
        ISemesterRepository semesterRepository)
    {
        _semesterRepository = semesterRepository;
    }

    public async Task<
        PaginationResult<ExpandoObject>>
        GetSemestersAsync(
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

        var includeCourses =
            expandSet.Contains("courses");

        var query =
            _semesterRepository
                .GetSemestersQueryable(
                    includeCourses);

        // SEARCH
        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(x =>
                x.SemesterName.Contains(search));
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
                    "semestername" =>
                        descending
                            ? query.OrderByDescending(
                                x => x.SemesterName)
                            : query.OrderBy(
                                x => x.SemesterName),

                    "startdate" =>
                        descending
                            ? query.OrderByDescending(
                                x => x.StartDate)
                            : query.OrderBy(
                                x => x.StartDate),

                    "enddate" =>
                        descending
                            ? query.OrderByDescending(
                                x => x.EndDate)
                            : query.OrderBy(
                                x => x.EndDate),

                    _ => query
                };
            }
        }

        var totalItems =
            await query.CountAsync();

        var semesters =
            await query
                .Skip((page - 1) * size)
                .Take(size)
                .Select(x =>
                    new SemesterExpandBusinessModel
                    {
                        SemesterId = x.SemesterId,
                        SemesterName = x.SemesterName,
                        StartDate = x.StartDate,
                        EndDate = x.EndDate,
                        Courses = includeCourses
                            ? x.Courses.Select(c => new
                            {
                                c.CourseId,
                                c.CourseName,
                                c.SemesterId,
                                c.SubjectId
                            }).ToList()
                            : null
                    })
                .ToListAsync();

        var shapedSemesters =
            semesters.ShapeData(fields);

        return new PaginationResult<
            ExpandoObject>
        {
            Items = shapedSemesters,
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

    public async Task<SemesterExpandBusinessModel>
        GetSemesterByIdAsync(int id)
    {
        var semester =
            await _semesterRepository
                .GetSemesterWithDetailsAsync(id);

        if (semester == null)
        {
            throw new NotFoundException(
                "Semester not found");
        }

        return new SemesterExpandBusinessModel
        {
            SemesterId = semester.SemesterId,
            SemesterName = semester.SemesterName,
            StartDate = semester.StartDate,
            EndDate = semester.EndDate,
            Courses = semester.Courses.Select(c => new
            {
                c.CourseId,
                c.CourseName,
                c.SemesterId,
                c.SubjectId
            }).ToList()
        };
    }

    public async Task<SemesterBusinessModel>
        CreateSemesterAsync(
            SemesterBusinessModel model)
    {
        var semester = new Semester
        {
            SemesterName = model.SemesterName,
            StartDate = model.StartDate,
            EndDate = model.EndDate
        };

        await _semesterRepository.AddAsync(semester);

        await _semesterRepository.SaveChangesAsync();

        return new SemesterBusinessModel
        {
            SemesterId = semester.SemesterId,
            SemesterName = semester.SemesterName,
            StartDate = semester.StartDate,
            EndDate = semester.EndDate
        };
    }

    public async Task<SemesterBusinessModel>
        UpdateSemesterAsync(
            int id,
            SemesterBusinessModel model)
    {
        var semester =
            await _semesterRepository.GetByIdAsync(id);

        if (semester == null)
        {
            throw new NotFoundException(
                "Semester not found");
        }

        semester.SemesterName = model.SemesterName;
        semester.StartDate = model.StartDate;
        semester.EndDate = model.EndDate;

        _semesterRepository.Update(semester);

        await _semesterRepository.SaveChangesAsync();

        return new SemesterBusinessModel
        {
            SemesterId = semester.SemesterId,
            SemesterName = semester.SemesterName,
            StartDate = semester.StartDate,
            EndDate = semester.EndDate
        };
    }

    public async Task DeleteSemesterAsync(int id)
    {
        var semester =
            await _semesterRepository.GetByIdAsync(id);

        if (semester == null)
        {
            throw new NotFoundException(
                "Semester not found");
        }

        _semesterRepository.Delete(semester);

        await _semesterRepository.SaveChangesAsync();
    }
}
