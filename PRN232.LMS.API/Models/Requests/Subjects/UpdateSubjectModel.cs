using System.ComponentModel.DataAnnotations;

namespace PRN232.LMS.API.Models.Requests.Subjects;

public class UpdateSubjectModel
{
    [StringLength(20)]
    public string? SubjectCode { get; set; }

    [StringLength(100)]
    public string? SubjectName { get; set; }

    public int? Credit { get; set; }
}
