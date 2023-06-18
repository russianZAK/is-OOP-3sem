namespace Backups.Exceptions;

public class FileException : Exception
{
    private FileException(string? message)
       : base(message)
    {
    }

    public static FileException InvalidPath()
    {
        return new FileException("path is invalid");
    }

    public static FileException InvalidName()
    {
        return new FileException("name is invalid");
    }
}
