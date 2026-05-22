using System.ComponentModel.DataAnnotations;

namespace PRN232.LMS.API.Models.Requests.Courses;

public class CreateCourseModel
{
    [Required]
    [StringLength(100)]
    public string CourseName { get; set; } = null!;

    [Required]
    public int SemesterId { get; set; }

    [Required]
    public int SubjectId { get; set; }
}
