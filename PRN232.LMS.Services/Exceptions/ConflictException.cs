namespace PRN232.LMS.Services.Exceptions;

public class ConflictException : Exception
{
    public ConflictException(string message)
        : base(message)
    {
    }
}