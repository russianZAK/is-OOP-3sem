namespace Backups.Extra.Exceptions;

public class ExtendedRepositoryException : Exception
{
    private ExtendedRepositoryException(string? message)
      : base(message)
    {
    }

    public static ExtendedRepositoryException InvalidPath()
    {
        return new ExtendedRepositoryException("path is invalid");
    }

    public static ExtendedRepositoryException InvalidBackupName()
    {
        return new ExtendedRepositoryException("Backup name is invalid");
    }

    public static ExtendedRepositoryException InvalidStorages()
    {
        return new ExtendedRepositoryException("Storages are invalid");
    }

    public static ExtendedRepositoryException InvalidProvider()
    {
        return new ExtendedRepositoryException("provider is invalid");
    }

    public static ExtendedRepositoryException InvalidRestorePoint()
    {
        return new ExtendedRepositoryException("restore point is invalid");
    }

    public static ExtendedRepositoryException InvalidRepository()
    {
        return new ExtendedRepositoryException("repository is invalid");
    }
}
