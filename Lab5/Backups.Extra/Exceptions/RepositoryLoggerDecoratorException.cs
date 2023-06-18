namespace Backups.Extra.Exceptions;

public class RepositoryLoggerDecoratorException : Exception
{
    private RepositoryLoggerDecoratorException(string? message)
      : base(message)
    {
    }

    public static RepositoryLoggerDecoratorException InvalidRepository()
    {
        return new RepositoryLoggerDecoratorException("repository is invalid");
    }

    public static RepositoryLoggerDecoratorException InvalidLogger()
    {
        return new RepositoryLoggerDecoratorException("logger is invalid");
    }

    public static RepositoryLoggerDecoratorException InvalidMerge()
    {
        return new RepositoryLoggerDecoratorException("merge is invalid");
    }

    public static RepositoryLoggerDecoratorException InvalidBackupName()
    {
        return new RepositoryLoggerDecoratorException("backup name is invalid");
    }

    public static RepositoryLoggerDecoratorException InvalidObject()
    {
        return new RepositoryLoggerDecoratorException("object is invalid");
    }

    public static RepositoryLoggerDecoratorException InvalidRestorePoint()
    {
        return new RepositoryLoggerDecoratorException("restorepoint is invalid");
    }

    public static RepositoryLoggerDecoratorException InvalidObjectProvider()
    {
        return new RepositoryLoggerDecoratorException("object provider is invalid");
    }

    public static RepositoryLoggerDecoratorException InvalidStorages()
    {
        return new RepositoryLoggerDecoratorException("storages are invalid");
    }
}
