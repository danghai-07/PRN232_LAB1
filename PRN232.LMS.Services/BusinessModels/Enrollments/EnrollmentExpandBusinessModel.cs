namespace PRN232.LMS.Services
    .BusinessModels.Enrollments;

public class EnrollmentExpandBusinessModel
    : EnrollmentBusinessModel
{
    public object? Student { get; set; }

    public object? Course { get; set; }
}
