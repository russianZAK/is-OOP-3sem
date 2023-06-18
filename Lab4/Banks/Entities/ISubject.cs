namespace Banks.Entities;

public interface ISubject
{
    void Attach(IWatcher watcher);

    void Detach(IWatcher watcher);

    void ChangeDate(DateTime time);
}
