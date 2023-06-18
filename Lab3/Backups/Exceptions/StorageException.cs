namespace Backups.Exceptions;

public class StorageException : Exception
{
    private StorageException(string? message)
       : base(message)
    {
    }

    public static StorageException InvalidObjects()
    {
        return new StorageException("objects are invalid");
    }

    public static StorageException InvalidPath()
    {
        return new StorageException("path is invalid");
    }

    public static StorageException InvalidStorage()
    {
        return new StorageException("storage is invalid");
    }
}
