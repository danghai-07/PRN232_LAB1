using System.ComponentModel.DataAnnotations;

namespace PRN232.LMS.API.Models.Requests.Students;

public class CreateStudentModel
{
    [Required]
    [StringLength(100)]
    public string FullName { get; set; } = null!;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;

    [Required]
    public DateTime DateOfBirth { get; set; }
}