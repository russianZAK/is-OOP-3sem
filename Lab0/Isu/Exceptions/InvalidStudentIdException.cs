namespace Isu.Exceptions;

public class InvalidStudentIdException : Exception
{
    public InvalidStudentIdException(int id)
    {
        Id = id;
    }

    public int Id { get; set; }
}