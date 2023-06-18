namespace Backups.Interfaces;

public interface IObjectProvider
{
    IReadOnlyCollection<IObject> Objects { get; }
    IObject AddNewObject(string path);
}
