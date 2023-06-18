using Banks.Exceptions;

namespace Banks.Notifications;

public class ChangeRestrictionForNotVerifiedCustomersNotification : INotification
{
    public ChangeRestrictionForNotVerifiedCustomersNotification(string message)
    {
        if (message == null) throw NotificationException.InvalidMessage();
        Message = message;
    }

    public string Message { get; }
}
