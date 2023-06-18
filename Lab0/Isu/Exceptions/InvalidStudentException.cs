namespace Isu.Exceptions;

public class InvalidStudentException : InvalidGroupException
{
    public InvalidStudentException(string? textMessage)
        : base(textMessage)
    {
    }
}
