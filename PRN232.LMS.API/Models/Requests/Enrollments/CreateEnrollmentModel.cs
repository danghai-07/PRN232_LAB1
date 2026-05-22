using System.ComponentModel.DataAnnotations;

namespace PRN232.LMS.API.Models.Requests.Enrollments;

public class CreateEnrollmentModel
{
    [Required]
    public int StudentId { get; set; }

    [Required]
    public int CourseId { get; set; }

    [Required]
    public DateTime EnrollDate { get; set; }

    [Required]
    [StringLength(20)]
    public string Status { get; set; } = null!;
}
