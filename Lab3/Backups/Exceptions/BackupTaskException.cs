namespace Backups.Exceptions;

public class BackupTaskException : Exception
{
    private BackupTaskException(string? message)
       : base(message)
    {
    }

    public static BackupTaskException InvalidName()
    {
        return new BackupTaskException("name is invalid");
    }

    public static BackupTaskException InvalidRepository()
    {
        return new BackupTaskException("repository is invalid");
    }

    public static BackupTaskException InvalidStorageAlgorithm()
    {
        return new BackupTaskException("storage algorithm is invalid");
    }

    public static BackupTaskException InvalidObject()
    {
        return new BackupTaskException("object is invalid");
    }

    public static BackupTaskException ObjectsDoesntExist()
    {
        return new BackupTaskException("object doesn't exist");
    }

    public static BackupTaskException RestorePointDoesntExist()
    {
        return new BackupTaskException("restore point doesn't exist");
    }
}
