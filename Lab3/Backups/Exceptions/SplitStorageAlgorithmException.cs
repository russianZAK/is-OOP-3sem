namespace Backups.Exceptions;

public class SplitStorageAlgorithmException : Exception
{
    private SplitStorageAlgorithmException(string? message)
       : base(message)
    {
    }

    public static SplitStorageAlgorithmException InvalidObjects()
    {
        return new SplitStorageAlgorithmException("objects are invalid");
    }

    public static SplitStorageAlgorithmException InvalidBackupPath()
    {
        return new SplitStorageAlgorithmException("backup path is invalid");
    }

    public static SplitStorageAlgorithmException InvalidId(int id)
    {
        return new SplitStorageAlgorithmException($"{id} is invalid");
    }
}
