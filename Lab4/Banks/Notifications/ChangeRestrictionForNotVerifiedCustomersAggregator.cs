using Banks.Exceptions;
namespace Banks.Notifications;

public class ChangeRestrictionForNotVerifiedCustomersAggregator : Models.IObservable<INotification>
{
    private readonly List<Models.IObserver<INotification>> _observers;

    public ChangeRestrictionForNotVerifiedCustomersAggregator()
    {
        _observers = new List<Models.IObserver<INotification>>();
    }

    public void Notify(INotification payload)
    {
        if (payload == null) throw AggregatorException.InvalidPayload();

        foreach (Models.IObserver<INotification> observer in _observers)
        {
            observer.Update(payload);
        }
    }

    public void Subscribe(Models.IObserver<INotification> observer)
    {
        if (observer == null) throw AggregatorException.InvalidObserver();
        _observers.Add(observer);
    }

    public void Unsubscribe(Models.IObserver<INotification> observer)
    {
        if (observer == null) throw AggregatorException.InvalidObserver();
        _observers.Remove(observer);
    }
}
