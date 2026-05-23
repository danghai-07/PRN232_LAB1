using System.ComponentModel.DataAnnotations;

namespace PRN232.LMS.API.Models.Requests.Subjects;

public class CreateSubjectModel
{
    [Required]
    [StringLength(20)]
    public string SubjectCode { get; set; } = null!;

    [Required]
    [StringLength(100)]
    public string SubjectName { get; set; } = null!;

    [Required]
    public int Credit { get; set; }
}
