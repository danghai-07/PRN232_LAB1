using Microsoft.AspNetCore.Mvc;
using PRN232.LMS.API.Common;
using PRN232.LMS.API.Models.Requests.Subjects;
using PRN232.LMS.API.Models.Responses.Subjects;
using PRN232.LMS.Services.BusinessModels.Subjects;
using PRN232.LMS.Services.Interfaces;

namespace PRN232.LMS.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SubjectsController : ControllerBase
{
    private readonly ISubjectService _subjectService;

    public SubjectsController(
        ISubjectService subjectService)
    {
        _subjectService = subjectService;
    }

    [HttpGet]
    public async Task<IActionResult>
        GetSubjects(
            string? search,
            string? sort,
            [FromQuery] string? fields,
            [FromQuery] string? expand,
            int page = 1,
            int size = 10)
    {
        var result =
            await _subjectService
                .GetSubjectsAsync(
                    search,
                    sort,
                    fields,
                    expand,
                    page,
                    size);

        var response = new
        {
            Items = result.Items,
            result.Pagination
        };

        return Ok(
            new ApiResponse<object>
            {
                Success = true,
                Message =
                    "Get subjects successfully",
                Data = response,
                Errors = null
            });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult>
        GetSubjectById(int id)
    {
        var subject =
            await _subjectService
                .GetSubjectByIdAsync(id);

        var response = new SubjectResponseModel
        {
            SubjectId = subject.SubjectId,
            SubjectCode = subject.SubjectCode,
            SubjectName = subject.SubjectName,
            Credit = subject.Credit,
            Courses = subject.Courses
        };

        return Ok(
            new ApiResponse<SubjectResponseModel>
            {
                Success = true,
                Message =
                    "Get subject successfully",
                Data = response,
                Errors = null
            });
    }

    [HttpPost]
    public async Task<IActionResult>
        CreateSubject(
            CreateSubjectModel request)
    {
        var businessModel =
            new SubjectBusinessModel
            {
                SubjectCode = request.SubjectCode,
                SubjectName = request.SubjectName,
                Credit = request.Credit
            };

        var subject =
            await _subjectService
                .CreateSubjectAsync(
                    businessModel);

        var response = new SubjectResponseModel
        {
            SubjectId = subject.SubjectId,
            SubjectCode = subject.SubjectCode,
            SubjectName = subject.SubjectName,
            Credit = subject.Credit
        };

        return StatusCode(
            StatusCodes.Status201Created,
            new ApiResponse<SubjectResponseModel>
            {
                Success = true,
                Message =
                    "Create subject successfully",
                Data = response,
                Errors = null
            });
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult>
        UpdateSubject(
            int id,
            UpdateSubjectModel request)
    {
        var currentSubject =
            await _subjectService
                .GetSubjectByIdAsync(id);

        var businessModel =
            new SubjectBusinessModel
            {
                SubjectId = id,
                SubjectCode =
                    request.SubjectCode
                    ?? currentSubject.SubjectCode,
                SubjectName =
                    request.SubjectName
                    ?? currentSubject.SubjectName,
                Credit =
                    request.Credit
                    ?? currentSubject.Credit
            };

        var updatedSubject =
            await _subjectService
                .UpdateSubjectAsync(
                    id,
                    businessModel);

        var response = new SubjectResponseModel
        {
            SubjectId = updatedSubject.SubjectId,
            SubjectCode = updatedSubject.SubjectCode,
            SubjectName = updatedSubject.SubjectName,
            Credit = updatedSubject.Credit
        };

        return Ok(
            new ApiResponse<SubjectResponseModel>
            {
                Success = true,
                Message =
                    "Update subject successfully",
                Data = response,
                Errors = null
            });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult>
        DeleteSubject(int id)
    {
        await _subjectService
            .DeleteSubjectAsync(id);

        return Ok(
            new ApiResponse<object>
            {
                Success = true,
                Message =
                    "Delete subject successfully",
                Data = null,
                Errors = null
            });
    }
}
