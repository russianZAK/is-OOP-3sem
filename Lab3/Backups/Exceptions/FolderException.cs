namespace Backups.Exceptions;

public class FolderException : Exception
{
    private FolderException(string? message)
       : base(message)
    {
    }

    public static FolderException InvalidPath()
    {
        return new FolderException("path is invalid");
    }

    public static FolderException InvalidName()
    {
        return new FolderException("name is invalid");
    }
}