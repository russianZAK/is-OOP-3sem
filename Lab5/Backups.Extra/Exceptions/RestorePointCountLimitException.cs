namespace Backups.Extra.Exceptions;

public class RestorePointCountLimitException : Exception
{
    private RestorePointCountLimitException(string? message)
      : base(message)
    {
    }

    public static RestorePointCountLimitException InvalidLimit(int count)
    {
        return new RestorePointCountLimitException($"{count} is invalid");
    }
}
