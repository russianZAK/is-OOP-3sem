namespace Backups.Extra.Exceptions;

public class MergeException : Exception
{
    private MergeException(string? message)
      : base(message)
    {
    }

    public static MergeException InvalidRestorePointLimit()
    {
        return new MergeException("restore point limit is invalid");
    }

    public static MergeException InvalidRestorePoint()
    {
        return new MergeException("restorepoint is invalid");
    }

    public static MergeException InvalidMerging()
    {
        return new MergeException("all restorepoints will be deleted");
    }
}
