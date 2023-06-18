namespace Backups.Extra.Exceptions;

public class LoggerException : Exception
{
    private LoggerException(string? message)
      : base(message)
    {
    }

    public static LoggerException InvalidPath()
    {
        return new LoggerException("path is invalid");
    }

    public static LoggerException InvalidMessage()
    {
        return new LoggerException("message is invalid");
    }
}
