using Isu.Entities;

namespace Isu.Extra.Exceptions;

public class OGNPGroupException : Exception
{
    private OGNPGroupException(string? message)
       : base(message)
    {
    }

    public static OGNPGroupException InvalidGroup()
    {
        throw new OGNPGroupException("group is invalid");
    }

    public static OGNPGroupException GroupIsFull()
    {
        throw new OGNPGroupException("group is full");
    }

    public static OGNPGroupException StudentIsAlreadyExists(Student student)
    {
        throw new OGNPGroupException($"{student.FirstName + " " + student.FirstName} is already exists");
    }

    public static OGNPGroupException StudentDoesntExist(Student student)
    {
        throw new OGNPGroupException($"{student.FirstName + " " + student.FirstName} doesnt exist");
    }
}
