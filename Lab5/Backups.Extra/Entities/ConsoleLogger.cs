using Backups.Extra.Exceptions;

namespace Backups.Extra.Entities;

public class ConsoleLogger : ILogger
{
    public ConsoleLogger(bool isUseDateTime)
    {
        IsUseDateTime = isUseDateTime;
    }

    public bool IsUseDateTime { get; }

    public void Logging(string message)
    {
        if (message == null) throw LoggerException.InvalidMessage();

        if (IsUseDateTime)
        {
            Console.WriteLine(DateTime.Now + " " + message);
        }
        else
        {
            Console.WriteLine(message);
        }
    }
}
