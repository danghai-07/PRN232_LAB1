using Microsoft.AspNetCore.Mvc;
using PRN232.LMS.API.Common;
using PRN232.LMS.API.Models.Requests.Semesters;
using PRN232.LMS.API.Models.Responses.Semesters;
using PRN232.LMS.Services.BusinessModels.Semesters;
using PRN232.LMS.Services.Interfaces;

namespace PRN232.LMS.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SemestersController : ControllerBase
{
    private readonly ISemesterService _semesterService;

    public SemestersController(
        ISemesterService semesterService)
    {
        _semesterService = semesterService;
    }

    [HttpGet]
    public async Task<IActionResult>
        GetSemesters(
            string? search,
            string? sort,
            [FromQuery] string? fields,
            [FromQuery] string? expand,
            int page = 1,
            int size = 10)
    {
        var result =
            await _semesterService
                .GetSemestersAsync(
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
                    "Get semesters successfully",
                Data = response,
                Errors = null
            });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult>
        GetSemesterById(int id)
    {
        var semester =
            await _semesterService
                .GetSemesterByIdAsync(id);

        var response = new SemesterResponseModel
        {
            SemesterId = semester.SemesterId,
            SemesterName = semester.SemesterName,
            StartDate = semester.StartDate,
            EndDate = semester.EndDate,
            Courses = semester.Courses
        };

        return Ok(
            new ApiResponse<SemesterResponseModel>
            {
                Success = true,
                Message =
                    "Get semester successfully",
                Data = response,
                Errors = null
            });
    }

    [HttpPost]
    public async Task<IActionResult>
        CreateSemester(
            CreateSemesterModel request)
    {
        var businessModel =
            new SemesterBusinessModel
            {
                SemesterName = request.SemesterName,
                StartDate = request.StartDate,
                EndDate = request.EndDate
            };

        var semester =
            await _semesterService
                .CreateSemesterAsync(
                    businessModel);

        var response = new SemesterResponseModel
        {
            SemesterId = semester.SemesterId,
            SemesterName = semester.SemesterName,
            StartDate = semester.StartDate,
            EndDate = semester.EndDate
        };

        return StatusCode(
            StatusCodes.Status201Created,
            new ApiResponse<SemesterResponseModel>
            {
                Success = true,
                Message =
                    "Create semester successfully",
                Data = response,
                Errors = null
            });
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult>
        UpdateSemester(
            int id,
            UpdateSemesterModel request)
    {
        var currentSemester =
            await _semesterService
                .GetSemesterByIdAsync(id);

        var businessModel =
            new SemesterBusinessModel
            {
                SemesterId = id,
                SemesterName =
                    request.SemesterName
                    ?? currentSemester.SemesterName,
                StartDate =
                    request.StartDate
                    ?? currentSemester.StartDate,
                EndDate =
                    request.EndDate
                    ?? currentSemester.EndDate
            };

        var updatedSemester =
            await _semesterService
                .UpdateSemesterAsync(
                    id,
                    businessModel);

        var response = new SemesterResponseModel
        {
            SemesterId = updatedSemester.SemesterId,
            SemesterName = updatedSemester.SemesterName,
            StartDate = updatedSemester.StartDate,
            EndDate = updatedSemester.EndDate
        };

        return Ok(
            new ApiResponse<SemesterResponseModel>
            {
                Success = true,
                Message =
                    "Update semester successfully",
                Data = response,
                Errors = null
            });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult>
        DeleteSemester(int id)
    {
        await _semesterService
            .DeleteSemesterAsync(id);

        return Ok(
            new ApiResponse<object>
            {
                Success = true,
                Message =
                    "Delete semester successfully",
                Data = null,
                Errors = null
            });
    }
}
