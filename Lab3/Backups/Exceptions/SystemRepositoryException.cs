namespace Backups.Exceptions;

public class SystemRepositoryException : Exception
{
    private SystemRepositoryException(string? message)
       : base(message)
    {
    }

    public static SystemRepositoryException InvalidPath()
    {
        return new SystemRepositoryException("path is invalid");
    }

    public static SystemRepositoryException InvalidBackupName()
    {
        return new SystemRepositoryException("Backup name is invalid");
    }

    public static SystemRepositoryException InvalidStorages()
    {
        return new SystemRepositoryException("Storages are invalid");
    }

    public static SystemRepositoryException InvalidProvider()
    {
        return new SystemRepositoryException("provider is invalid");
    }

    public static SystemRepositoryException InvalidRestorePoint()
    {
        return new SystemRepositoryException("restore point is invalid");
    }
}
