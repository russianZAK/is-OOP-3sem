namespace Backups.Extra.Exceptions;

public class ExtendedBackupTaskException : Exception
{
    private ExtendedBackupTaskException(string? message)
      : base(message)
    {
    }

    public static ExtendedBackupTaskException InvalidBackupName()
    {
        return new ExtendedBackupTaskException("backupname is invalid");
    }

    public static ExtendedBackupTaskException InvalidRepository()
    {
        return new ExtendedBackupTaskException("repository is invalid");
    }

    public static ExtendedBackupTaskException InvalidStorageAlgorithm()
    {
        return new ExtendedBackupTaskException("storage algorithm is invalid");
    }
}
