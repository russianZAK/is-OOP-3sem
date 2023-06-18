namespace Banks.Exceptions;

public class MediatorException : Exception
{
    private MediatorException(string? message)
  : base(message)
    { }

    public static MediatorException InvalidClient()
    {
        return new MediatorException("client is invalid");
    }

    public static MediatorException InvalidNotification()
    {
        return new MediatorException("notification is invalid");
    }
}
