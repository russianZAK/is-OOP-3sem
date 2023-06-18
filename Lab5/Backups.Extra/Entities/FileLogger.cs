using Backups.Extra.Exceptions;

namespace Backups.Extra.Entities;

public class FileLogger : ILogger
{
    public FileLogger(bool isUseDateTime, string pathToFile)
    {
        if (pathToFile == null) throw LoggerException.InvalidPath();
        IsUseDateTime = isUseDateTime;
        PathToFile = pathToFile;
    }

    public bool IsUseDateTime { get; }
    public string PathToFile { get; }

    public void Logging(string message)
    {
        if (message == null) throw LoggerException.InvalidMessage();

        if (IsUseDateTime)
        {
            using (var streamWriter = new StreamWriter(PathToFile, true))
            {
                streamWriter.WriteLine(DateTime.Now + " " + message);
            }
        }
        else
        {
            using (var streamWriter = new StreamWriter(PathToFile, true))
            {
                streamWriter.WriteLine(message);
            }
        }
    }
}
