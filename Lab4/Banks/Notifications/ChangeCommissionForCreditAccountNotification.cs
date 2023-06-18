using Banks.Exceptions;

namespace Banks.Notifications;

public class ChangeCommissionForCreditAccountNotification : INotification
{
    public ChangeCommissionForCreditAccountNotification(string message)
    {
        if (message == null) throw NotificationException.InvalidMessage();
        Message = message;
    }

    public string Message { get; }
}
