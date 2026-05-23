using System.ComponentModel.DataAnnotations;

namespace PRN232.LMS.API.Models.Requests.Enrollments;

public class UpdateEnrollmentModel
{
    public int? StudentId { get; set; }

    public int? CourseId { get; set; }

    public DateTime? EnrollDate { get; set; }

    [StringLength(20)]
    public string? Status { get; set; }
}
