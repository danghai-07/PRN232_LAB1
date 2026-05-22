using System.ComponentModel.DataAnnotations;

namespace PRN232.LMS.API.Models.Requests.Students;

public class UpdateStudentModel
{
    [StringLength(100)]
    public string? FullName { get; set; }

    [EmailAddress]
    public string? Email { get; set; }

    public DateTime? DateOfBirth { get; set; }
}