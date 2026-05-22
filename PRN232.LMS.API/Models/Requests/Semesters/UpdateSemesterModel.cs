using System.ComponentModel.DataAnnotations;

namespace PRN232.LMS.API.Models.Requests.Semesters;

public class UpdateSemesterModel
{
    [StringLength(100)]
    public string? SemesterName { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }
}
