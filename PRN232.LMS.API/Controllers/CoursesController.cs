using Microsoft.AspNetCore.Mvc;
using PRN232.LMS.API.Common;
using PRN232.LMS.API.Models.Requests.Courses;
using PRN232.LMS.API.Models.Responses.Courses;
using PRN232.LMS.Services.BusinessModels.Courses;
using PRN232.LMS.Services.Interfaces;

namespace PRN232.LMS.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CoursesController : ControllerBase
{
    private readonly ICourseService _courseService;

    public CoursesController(
        ICourseService courseService)
    {
        _courseService = courseService;
    }

    [HttpGet]
    public async Task<IActionResult>
        GetCourses(
            string? search,
            string? sort,
            [FromQuery] string? fields,
            [FromQuery] string? expand,
            int page = 1,
            int size = 10)
    {
        size = Math.Clamp(size, 1, 100);
        page = Math.Max(1, page);

        var result =
            await _courseService
                .GetCoursesAsync(
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
                    "Get courses successfully",
                Data = response,
                Errors = null
            });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult>
        GetCourseById(int id)
    {
        var course =
            await _courseService
                .GetCourseByIdAsync(id);

        var response = new CourseResponseModel
        {
            CourseId = course.CourseId,
            CourseName = course.CourseName,
            SemesterId = course.SemesterId,
            SubjectId = course.SubjectId,
            Semester = course.Semester,
            Subject = course.Subject,
            Enrollments = course.Enrollments
        };

        return Ok(
            new ApiResponse<CourseResponseModel>
            {
                Success = true,
                Message =
                    "Get course successfully",
                Data = response,
                Errors = null
            });
    }

    [HttpPost]
    public async Task<IActionResult>
        CreateCourse(
            CreateCourseModel request)
    {
        var businessModel =
            new CourseBusinessModel
            {
                CourseName = request.CourseName,
                SemesterId = request.SemesterId,
                SubjectId = request.SubjectId
            };

        var course =
            await _courseService
                .CreateCourseAsync(
                    businessModel);

        var response = new CourseResponseModel
        {
            CourseId = course.CourseId,
            CourseName = course.CourseName,
            SemesterId = course.SemesterId,
            SubjectId = course.SubjectId
        };

        return StatusCode(
            StatusCodes.Status201Created,
            new ApiResponse<CourseResponseModel>
            {
                Success = true,
                Message =
                    "Create course successfully",
                Data = response,
                Errors = null
            });
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult>
        UpdateCourse(
            int id,
            UpdateCourseModel request)
    {
        var currentCourse =
            await _courseService
                .GetCourseByIdAsync(id);

        var businessModel =
            new CourseBusinessModel
            {
                CourseId = id,
                CourseName =
                    request.CourseName
                    ?? currentCourse.CourseName,
                SemesterId =
                    request.SemesterId
                    ?? currentCourse.SemesterId,
                SubjectId =
                    request.SubjectId
                    ?? currentCourse.SubjectId
            };

        var updatedCourse =
            await _courseService
                .UpdateCourseAsync(
                    id,
                    businessModel);

        var response = new CourseResponseModel
        {
            CourseId = updatedCourse.CourseId,
            CourseName = updatedCourse.CourseName,
            SemesterId = updatedCourse.SemesterId,
            SubjectId = updatedCourse.SubjectId
        };

        return Ok(
            new ApiResponse<CourseResponseModel>
            {
                Success = true,
                Message =
                    "Update course successfully",
                Data = response,
                Errors = null
            });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult>
        DeleteCourse(int id)
    {
        await _courseService
            .DeleteCourseAsync(id);

        return Ok(
            new ApiResponse<object>
            {
                Success = true,
                Message =
                    "Delete course successfully",
                Data = null,
                Errors = null
            });
    }
}
