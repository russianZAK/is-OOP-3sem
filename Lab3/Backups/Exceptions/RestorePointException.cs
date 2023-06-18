namespace Backups.Exceptions;

public class RestorePointException : Exception
{
    private RestorePointException(string? message)
       : base(message)
    {
    }

    public static RestorePointException InvalidId()
    {
        return new RestorePointException("id is invalid");
    }

    public static RestorePointException InvalidObjects()
    {
        return new RestorePointException("objects are invalid");
    }

    public static RestorePointException InvalidStorages()
    {
        return new RestorePointException("storages is invalid");
    }
}