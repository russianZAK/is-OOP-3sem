namespace Isu.Extra.Exceptions;

public class OGNPFlowException : Exception
{
    private OGNPFlowException(string? message)
      : base(message)
    {
    }

    public static OGNPFlowException InvalidOGNPFlow()
    {
        throw new OGNPFlowException("flow is invalid");
    }

    public static OGNPFlowException GroupIsAlreadyExists()
    {
        throw new OGNPFlowException("group is already exists");
    }

    public static OGNPFlowException GroupDoesntExist()
    {
        throw new OGNPFlowException("group doesn't exist");
    }

    public static OGNPFlowException IntersectionWithExistingSchedule()
    {
        throw new OGNPFlowException("lessons intersect with the existing schedule");
    }
}
