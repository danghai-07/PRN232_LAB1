using Microsoft.AspNetCore.Mvc;

using PRN232.LMS.API.Common;

using PRN232.LMS.API.Models
    .Requests.Students;

using PRN232.LMS.API.Models
    .Responses.Students;

using PRN232.LMS.Services.BusinessModels.Students;
using PRN232.LMS.Services.Interfaces;

namespace PRN232.LMS.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StudentsController : ControllerBase
{
    private readonly IStudentService
        _studentService;

    public StudentsController(
        IStudentService studentService)
    {
        _studentService = studentService;
    }

    [HttpGet]
    public async Task<IActionResult>
        GetStudents(
            string? search,
            string? sort,
            [FromQuery] string? fields,
            [FromQuery] string? expand,
            int page = 1,
            int size = 10)
    {
        var result =
            await _studentService
                .GetStudentsAsync(
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
                    "Get students successfully",

                Data = response,

                Errors = null
            });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult>
        GetStudentById(int id)
    {
        var student =
            await _studentService
                .GetStudentByIdAsync(id);

        var response =
            new StudentResponseModel
            {
                StudentId = student.StudentId,
                FullName = student.FullName,
                Email = student.Email,
                DateOfBirth =
                    student.DateOfBirth
            };

        return Ok(
            new ApiResponse<StudentResponseModel>
            {
                Success = true,
                Message =
                    "Get student successfully",

                Data = response,

                Errors = null
            });
    }

    [HttpPost]
    public async Task<IActionResult>
        CreateStudent(
            CreateStudentModel request)
    {
        var businessModel =
            new StudentBusinessModel
            {
                FullName = request.FullName,
                Email = request.Email,
                DateOfBirth =
                    request.DateOfBirth
            };

        var student =
            await _studentService
                .CreateStudentAsync(
                    businessModel);

        var response =
            new StudentResponseModel
            {
                StudentId = student.StudentId,
                FullName = student.FullName,
                Email = student.Email,
                DateOfBirth =
                    student.DateOfBirth
            };

        return StatusCode(
            StatusCodes.Status201Created,

            new ApiResponse<StudentResponseModel>
            {
                Success = true,
                Message =
                    "Create student successfully",

                Data = response,

                Errors = null
            });
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult>
        UpdateStudent(
            int id,
            UpdateStudentModel request)
    {
        var currentStudent =
            await _studentService
                .GetStudentByIdAsync(id);

        var businessModel =
            new StudentBusinessModel
            {
                StudentId = id,

                FullName =
                    request.FullName
                    ?? currentStudent.FullName,

                Email =
                    request.Email
                    ?? currentStudent.Email,

                DateOfBirth =
                    request.DateOfBirth
                    ?? currentStudent
                        .DateOfBirth
            };

        var updatedStudent =
            await _studentService
                .UpdateStudentAsync(
                    id,
                    businessModel);

        var response =
            new StudentResponseModel
            {
                StudentId =
                    updatedStudent.StudentId,

                FullName =
                    updatedStudent.FullName,

                Email =
                    updatedStudent.Email,

                DateOfBirth =
                    updatedStudent.DateOfBirth
            };

        return Ok(
            new ApiResponse<StudentResponseModel>
            {
                Success = true,
                Message =
                    "Update student successfully",

                Data = response,

                Errors = null
            });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult>
        DeleteStudent(int id)
    {
        await _studentService
            .DeleteStudentAsync(id);

        return Ok(
            new ApiResponse<object>
            {
                Success = true,
                Message =
                    "Delete student successfully",

                Data = null,

                Errors = null
            });
    }
}