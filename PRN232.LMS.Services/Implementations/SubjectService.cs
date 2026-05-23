using System.Dynamic;
using Microsoft.EntityFrameworkCore;
using PRN232.LMS.Repositories.Entities;
using PRN232.LMS.Repositories.Interfaces;
using PRN232.LMS.Services.BusinessModels.Common;
using PRN232.LMS.Services.BusinessModels.Subjects;
using PRN232.LMS.Services.Exceptions;
using PRN232.LMS.Services.Helpers;
using PRN232.LMS.Services.Interfaces;

namespace PRN232.LMS.Services.Implementations;

public class SubjectService : ISubjectService
{
    private readonly ISubjectRepository _subjectRepository;

    public SubjectService(
        ISubjectRepository subjectRepository)
    {
        _subjectRepository = subjectRepository;
    }

    public async Task<
        PaginationResult<ExpandoObject>>
        GetSubjectsAsync(
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
            _subjectRepository
                .GetSubjectsQueryable(
                    includeCourses);

        // SEARCH
        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(x =>
                x.SubjectCode.Contains(search)
                || x.SubjectName.Contains(search));
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
                    "subjectcode" =>
                        descending
                            ? query.OrderByDescending(
                                x => x.SubjectCode)
                            : query.OrderBy(
                                x => x.SubjectCode),

                    "subjectname" =>
                        descending
                            ? query.OrderByDescending(
                                x => x.SubjectName)
                            : query.OrderBy(
                                x => x.SubjectName),

                    "credit" =>
                        descending
                            ? query.OrderByDescending(
                                x => x.Credit)
                            : query.OrderBy(
                                x => x.Credit),

                    _ => query
                };
            }
        }

        var totalItems =
            await query.CountAsync();

        var subjects =
            await query
                .Skip((page - 1) * size)
                .Take(size)
                .Select(x =>
                    new SubjectExpandBusinessModel
                    {
                        SubjectId = x.SubjectId,
                        SubjectCode = x.SubjectCode,
                        SubjectName = x.SubjectName,
                        Credit = x.Credit,
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

        var shapedSubjects =
            subjects.ShapeData(fields);

        return new PaginationResult<
            ExpandoObject>
        {
            Items = shapedSubjects,
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

    public async Task<SubjectExpandBusinessModel>
        GetSubjectByIdAsync(int id)
    {
        var subject =
            await _subjectRepository
                .GetSubjectWithDetailsAsync(id);

        if (subject == null)
        {
            throw new NotFoundException(
                "Subject not found");
        }

        return new SubjectExpandBusinessModel
        {
            SubjectId = subject.SubjectId,
            SubjectCode = subject.SubjectCode,
            SubjectName = subject.SubjectName,
            Credit = subject.Credit,
            Courses = subject.Courses.Select(c => new
            {
                c.CourseId,
                c.CourseName,
                c.SemesterId,
                c.SubjectId
            }).ToList()
        };
    }

    public async Task<SubjectBusinessModel>
        CreateSubjectAsync(
            SubjectBusinessModel model)
    {
        var subject = new Subject
        {
            SubjectCode = model.SubjectCode,
            SubjectName = model.SubjectName,
            Credit = model.Credit
        };

        await _subjectRepository.AddAsync(subject);

        await _subjectRepository.SaveChangesAsync();

        return new SubjectBusinessModel
        {
            SubjectId = subject.SubjectId,
            SubjectCode = subject.SubjectCode,
            SubjectName = subject.SubjectName,
            Credit = subject.Credit
        };
    }

    public async Task<SubjectBusinessModel>
        UpdateSubjectAsync(
            int id,
            SubjectBusinessModel model)
    {
        var subject =
            await _subjectRepository.GetByIdAsync(id);

        if (subject == null)
        {
            throw new NotFoundException(
                "Subject not found");
        }

        subject.SubjectCode = model.SubjectCode;
        subject.SubjectName = model.SubjectName;
        subject.Credit = model.Credit;

        _subjectRepository.Update(subject);

        await _subjectRepository.SaveChangesAsync();

        return new SubjectBusinessModel
        {
            SubjectId = subject.SubjectId,
            SubjectCode = subject.SubjectCode,
            SubjectName = subject.SubjectName,
            Credit = subject.Credit
        };
    }

    public async Task DeleteSubjectAsync(int id)
    {
        var subject =
            await _subjectRepository.GetByIdAsync(id);

        if (subject == null)
        {
            throw new NotFoundException(
                "Subject not found");
        }

        _subjectRepository.Delete(subject);

        await _subjectRepository.SaveChangesAsync();
    }
}
