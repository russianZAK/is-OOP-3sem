namespace Backups.Extra.Exceptions;

public class BackupTaskDecoratorException : Exception
{
    private BackupTaskDecoratorException(string? message)
      : base(message)
    {
    }

    public static BackupTaskDecoratorException InvalidExtendedBackupTask()
    {
        return new BackupTaskDecoratorException("extendedbackuptask is invalid");
    }

    public static BackupTaskDecoratorException InvalidBackupTask()
    {
        return new BackupTaskDecoratorException("backuptask is invalid");
    }

    public static BackupTaskDecoratorException InvalidLogger()
    {
        return new BackupTaskDecoratorException("logger is invalid");
    }

    public static BackupTaskDecoratorException InvalidMerge()
    {
        return new BackupTaskDecoratorException("merge is invalid");
    }

    public static BackupTaskDecoratorException InvalidRestorePointLimit()
    {
        return new BackupTaskDecoratorException("restore point limit is invalid");
    }

    public static BackupTaskDecoratorException InvalidObject()
    {
        return new BackupTaskDecoratorException("object is invalid");
    }

    public static BackupTaskDecoratorException InvalidRestorePoint()
    {
        return new BackupTaskDecoratorException("restorepoint is invalid");
    }
}
