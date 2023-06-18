using Banks.Exceptions;

namespace Banks.Notifications;

public class ChangePercentsForDebitAccountNotification : INotification
{
    public ChangePercentsForDebitAccountNotification(string message)
    {
        if (message == null) throw NotificationException.InvalidMessage();
        Message = message;
    }

    public string Message { get; }
}
