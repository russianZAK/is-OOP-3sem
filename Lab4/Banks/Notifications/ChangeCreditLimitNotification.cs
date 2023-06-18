using Banks.Exceptions;

namespace Banks.Notifications;

public class ChangeCreditLimitNotification : INotification
{
    public ChangeCreditLimitNotification(string message)
    {
        if (message == null) throw NotificationException.InvalidMessage();
        Message = message;
    }

    public string Message { get; }
}
