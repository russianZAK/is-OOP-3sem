namespace Isu.Exceptions;

public class GroupDoesntExistsException : Exception
{
    public GroupDoesntExistsException(string groupName)
    {
        GroupName = groupName;
    }

    public string GroupName { get; set; }
}