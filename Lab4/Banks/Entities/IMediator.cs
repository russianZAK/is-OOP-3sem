using Banks.Notifications;

namespace Banks.Models;

public interface IMediator
{
    void Notify(Client client, INotification notification);
}
