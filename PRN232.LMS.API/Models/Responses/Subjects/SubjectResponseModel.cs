namespace PRN232.LMS.API.Models.Responses.Subjects;

public class SubjectResponseModel
{
    public int SubjectId { get; set; }

    public string SubjectCode { get; set; } = null!;

    public string SubjectName { get; set; } = null!;

    public int Credit { get; set; }

    public object? Courses { get; set; }
}
