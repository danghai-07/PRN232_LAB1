using System.ComponentModel.DataAnnotations;

namespace PRN232.LMS.API.Models.Requests.Semesters;

public class CreateSemesterModel
{
    [Required]
    [StringLength(100)]
    public string SemesterName { get; set; } = null!;

    [Required]
    public DateTime StartDate { get; set; }

    [Required]
    public DateTime EndDate { get; set; }
}
