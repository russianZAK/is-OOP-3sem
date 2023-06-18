namespace Isu.Exceptions;

public class StudentHasGroupAndGroupContainsStudentException : Exception
{
    public StudentHasGroupAndGroupContainsStudentException(string studentName)
    {
        StudentName = studentName;
    }

    public string StudentName { get; set; }
}