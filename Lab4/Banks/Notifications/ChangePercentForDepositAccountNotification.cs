using Banks.Exceptions;

namespace Banks.Notifications;

public class ChangePercentForDepositAccountNotification : INotification
{
    public ChangePercentForDepositAccountNotification(string message)
    {
        if (message == null) throw NotificationException.InvalidMessage();
        Message = message;
    }

    public string Message { get; }
}
