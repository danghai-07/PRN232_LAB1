namespace PRN232.LMS.API.Models.Responses.Courses;

public class CourseResponseModel
{
    public int CourseId { get; set; }

    public string CourseName { get; set; } = null!;

    public int SemesterId { get; set; }

    public int SubjectId { get; set; }

    public object? Semester { get; set; }

    public object? Subject { get; set; }

    public object? Enrollments { get; set; }
}
