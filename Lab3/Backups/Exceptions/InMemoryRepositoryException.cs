namespace Backups.Exceptions;

public class InMemoryRepositoryException : Exception
{
    private InMemoryRepositoryException(string? message)
       : base(message)
    {
    }

    public static InMemoryRepositoryException ObjectDoesntExist()
    {
        return new InMemoryRepositoryException("object doesn't exist");
    }

    public static InMemoryRepositoryException InvalidBackupName()
    {
        return new InMemoryRepositoryException("Backup name is invalid");
    }

    public static InMemoryRepositoryException InvalidStorages()
    {
        return new InMemoryRepositoryException("Storages are invalid");
    }

    public static InMemoryRepositoryException InvalidProvider()
    {
        return new InMemoryRepositoryException("provider is invalid");
    }

    public static InMemoryRepositoryException InvalidRestorePoint()
    {
        return new InMemoryRepositoryException("restore point is invalid");
    }
}
