using Microsoft.AspNetCore.Mvc;
using PRN232.LMS.API.Common;
using PRN232.LMS.API.Models.Requests.Enrollments;
using PRN232.LMS.API.Models.Responses.Enrollments;
using PRN232.LMS.Services.BusinessModels.Enrollments;
using PRN232.LMS.Services.Interfaces;

namespace PRN232.LMS.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EnrollmentsController : ControllerBase
{
    private readonly IEnrollmentService _enrollmentService;

    public EnrollmentsController(
        IEnrollmentService enrollmentService)
    {
        _enrollmentService = enrollmentService;
    }

    [HttpGet]
    public async Task<IActionResult>
        GetEnrollments(
            string? search,
            string? sort,
            [FromQuery] string? fields,
            [FromQuery] string? expand,
            int page = 1,
            int size = 10)
    {
        var result =
            await _enrollmentService
                .GetEnrollmentsAsync(
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
                    "Get enrollments successfully",
                Data = response,
                Errors = null
            });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult>
        GetEnrollmentById(int id)
    {
        var enrollment =
            await _enrollmentService
                .GetEnrollmentByIdAsync(id);

        var response = new EnrollmentResponseModel
        {
            EnrollmentId = enrollment.EnrollmentId,
            StudentId = enrollment.StudentId,
            CourseId = enrollment.CourseId,
            EnrollDate = enrollment.EnrollDate,
            Status = enrollment.Status,
            Student = enrollment.Student,
            Course = enrollment.Course
        };

        return Ok(
            new ApiResponse<EnrollmentResponseModel>
            {
                Success = true,
                Message =
                    "Get enrollment successfully",
                Data = response,
                Errors = null
            });
    }

    [HttpPost]
    public async Task<IActionResult>
        CreateEnrollment(
            CreateEnrollmentModel request)
    {
        var businessModel =
            new EnrollmentBusinessModel
            {
                StudentId = request.StudentId,
                CourseId = request.CourseId,
                EnrollDate = request.EnrollDate,
                Status = request.Status
            };

        var enrollment =
            await _enrollmentService
                .CreateEnrollmentAsync(
                    businessModel);

        var response = new EnrollmentResponseModel
        {
            EnrollmentId = enrollment.EnrollmentId,
            StudentId = enrollment.StudentId,
            CourseId = enrollment.CourseId,
            EnrollDate = enrollment.EnrollDate,
            Status = enrollment.Status
        };

        return StatusCode(
            StatusCodes.Status201Created,
            new ApiResponse<EnrollmentResponseModel>
            {
                Success = true,
                Message =
                    "Create enrollment successfully",
                Data = response,
                Errors = null
            });
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult>
        UpdateEnrollment(
            int id,
            UpdateEnrollmentModel request)
    {
        var currentEnrollment =
            await _enrollmentService
                .GetEnrollmentByIdAsync(id);

        var businessModel =
            new EnrollmentBusinessModel
            {
                EnrollmentId = id,
                StudentId =
                    request.StudentId
                    ?? currentEnrollment.StudentId,
                CourseId =
                    request.CourseId
                    ?? currentEnrollment.CourseId,
                EnrollDate =
                    request.EnrollDate
                    ?? currentEnrollment.EnrollDate,
                Status =
                    request.Status
                    ?? currentEnrollment.Status
            };

        var updatedEnrollment =
            await _enrollmentService
                .UpdateEnrollmentAsync(
                    id,
                    businessModel);

        var response = new EnrollmentResponseModel
        {
            EnrollmentId = updatedEnrollment.EnrollmentId,
            StudentId = updatedEnrollment.StudentId,
            CourseId = updatedEnrollment.CourseId,
            EnrollDate = updatedEnrollment.EnrollDate,
            Status = updatedEnrollment.Status
        };

        return Ok(
            new ApiResponse<EnrollmentResponseModel>
            {
                Success = true,
                Message =
                    "Update enrollment successfully",
                Data = response,
                Errors = null
            });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult>
        DeleteEnrollment(int id)
    {
        await _enrollmentService
            .DeleteEnrollmentAsync(id);

        return Ok(
            new ApiResponse<object>
            {
                Success = true,
                Message =
                    "Delete enrollment successfully",
                Data = null,
                Errors = null
            });
    }
}
