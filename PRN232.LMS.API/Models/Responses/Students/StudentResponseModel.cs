namespace PRN232.LMS.API.Models.Responses.Students;

public class StudentResponseModel
{
    public int StudentId { get; set; }

    public string FullName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public DateTime DateOfBirth { get; set; }

    public object? Enrollments { get; set; }
}