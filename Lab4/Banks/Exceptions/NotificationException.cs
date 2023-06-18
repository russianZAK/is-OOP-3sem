namespace Banks.Exceptions;

public class NotificationException : Exception
{
    private NotificationException(string? message)
   : base(message)
    { }

    public static NotificationException InvalidMessage()
    {
        return new NotificationException("message is invalid");
    }
}