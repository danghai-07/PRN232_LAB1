using System.ComponentModel.DataAnnotations;

namespace PRN232.LMS.API.Models.Requests.Courses;

public class UpdateCourseModel
{
    [StringLength(100)]
    public string? CourseName { get; set; }

    public int? SemesterId { get; set; }

    public int? SubjectId { get; set; }
}
