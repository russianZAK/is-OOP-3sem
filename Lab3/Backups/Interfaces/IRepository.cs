using Backups.Entities;

namespace Backups.Interfaces;

public interface IRepository
{
    public string Path { get; }
    public IReadOnlyCollection<Storage> Storages { get; }
    public IReadOnlyCollection<IObject> BackupObjects { get; }
    public void BackUp(IReadOnlyCollection<Storage> storages, string backupName);
    public void AddNewObjects(IObjectProvider objectProvider);
}
