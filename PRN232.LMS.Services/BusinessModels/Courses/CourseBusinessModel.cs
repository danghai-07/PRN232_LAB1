namespace PRN232.LMS.Services
    .BusinessModels.Courses;

public class CourseBusinessModel
{
    public int CourseId { get; set; }

    public string CourseName { get; set; }
        = string.Empty;

    public int SemesterId { get; set; }

    public int SubjectId { get; set; }
}
