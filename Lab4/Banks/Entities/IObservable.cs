namespace Banks.Models;

public interface IObservable<TPayload>
{
    void Subscribe(IObserver<TPayload> observer);
    void Unsubscribe(IObserver<TPayload> observer);
    void Notify(TPayload payload);
}
