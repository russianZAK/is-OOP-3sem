namespace Backups.Exceptions;

public class LeafException : Exception
{
    private LeafException(string? message)
       : base(message)
    {
    }

    public static LeafException ThisMethodCantBeUsed()
    {
        return new LeafException("leaf doesn't have this method");
    }
}
