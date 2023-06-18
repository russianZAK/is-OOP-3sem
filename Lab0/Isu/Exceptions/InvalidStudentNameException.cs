namespace Isu.Exceptions;

public class InvalidStudentNameException : Exception
{
    public InvalidStudentNameException(string? name)
    {
        Name = name;
    }

    public string? Name { get; set; }
}
