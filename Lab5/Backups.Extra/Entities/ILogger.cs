namespace Backups.Extra.Entities;

public interface ILogger
{
    bool IsUseDateTime { get; }
    void Logging(string message);
}
