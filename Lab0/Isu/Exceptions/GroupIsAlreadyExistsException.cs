namespace Isu.Exceptions;

public class GroupIsAlreadyExistsException : Exception
{
    public GroupIsAlreadyExistsException(string groupName)
    {
        GroupName = groupName;
    }

    public string GroupName { get; set; }
}