namespace Banks.Models;

public interface IObserver<TPayload>
{
    void Update(TPayload payload);
}
