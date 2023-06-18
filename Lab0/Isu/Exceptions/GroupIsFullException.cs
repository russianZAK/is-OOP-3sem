namespace Isu.Exceptions;

public class GroupIsFullException : Exception
{
    public GroupIsFullException(string groupName)
    {
        GroupName = groupName;
    }

    public string GroupName { get; set; }
}