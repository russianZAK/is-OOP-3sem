namespace Backups.Exceptions;

public class SingleStorageAlgorithmException : Exception
{
    private SingleStorageAlgorithmException(string? message)
       : base(message)
    {
    }

    public static SingleStorageAlgorithmException InvalidObjects()
    {
        return new SingleStorageAlgorithmException("objects are invalid");
    }

    public static SingleStorageAlgorithmException InvalidBackupPath()
    {
        return new SingleStorageAlgorithmException("backup path is invalid");
    }

    public static SingleStorageAlgorithmException InvalidId(int id)
    {
        return new SingleStorageAlgorithmException($"{id} is invalid");
    }
}
