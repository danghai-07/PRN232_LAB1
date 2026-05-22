namespace PRN232.LMS.API.Models.Responses.Enrollments;

public class EnrollmentResponseModel
{
    public int EnrollmentId { get; set; }

    public int StudentId { get; set; }

    public int CourseId { get; set; }

    public DateTime EnrollDate { get; set; }

    public string Status { get; set; } = null!;

    public object? Student { get; set; }

    public object? Course { get; set; }
}
