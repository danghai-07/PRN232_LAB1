namespace PRN232.LMS.Services
    .BusinessModels.Courses;

public class CourseExpandBusinessModel
    : CourseBusinessModel
{
    public object? Semester { get; set; }

    public object? Subject { get; set; }

    public object? Enrollments { get; set; }
}
