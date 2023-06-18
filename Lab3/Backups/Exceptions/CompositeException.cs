namespace Backups.Exceptions;

public class CompositeException : Exception
{
    private CompositeException(string? message)
       : base(message)
    {
    }

    public static CompositeException InvalidComponentName()
    {
        return new CompositeException("component name is invalid");
    }

    public static CompositeException InvalidDepth(int depth)
    {
        return new CompositeException($"{depth} is invalid");
    }
}