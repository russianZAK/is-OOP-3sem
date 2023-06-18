namespace Backups.Exceptions;

public class BackupException : Exception
{
    private BackupException(string? message)
       : base(message)
    {
    }

    public static BackupException InvalidRestorePoint()
    {
        return new BackupException("restorepoint is invalid");
    }

    public static BackupException InvalidId(int id)
    {
        return new BackupException($"{id} is invalid");
    }
}