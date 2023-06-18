namespace Backups.Exceptions;

public class ComponentException : Exception
{
    private ComponentException(string? message)
       : base(message)
    {
    }

    public static ComponentException InvalidComponent()
    {
        return new ComponentException("component is invalid");
    }

    public static ComponentException InvalidOperationException()
    {
        return new ComponentException("this operation is invalid for leaf");
    }
}