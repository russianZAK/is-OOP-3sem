using Backups.Exceptions;
using Backups.Interfaces;

namespace Backups.Entities;

public class RestorePoint
{
    private readonly List<IObject> _backupObjects;
    private readonly List<Storage> _storages;
    private int _id;

    public RestorePoint(IReadOnlyCollection<IObject> objects, int id, IEnumerable<Storage> storages)
    {
        if (objects == null) throw RestorePointException.InvalidObjects();
        if (storages == null) throw RestorePointException.InvalidStorages();
        if (id < 0) throw RestorePointException.InvalidId();

        _backupObjects = objects.ToList();
        DateTime = DateTime.Now;
        _id = id;
        _storages = storages.ToList();
    }

    public IReadOnlyCollection<IObject> BackupObjects => _backupObjects;
    public int Id => _id;
    public IReadOnlyCollection<Storage> Storages => _storages;
    public DateTime DateTime { get; private set; }

    public void DeleteStorage(Storage storage)
    {
        if (storage == null) throw StorageException.InvalidStorage();
        _storages.Remove(storage);
    }

    public void AddStorage(Storage storage)
    {
        if (storage == null) throw StorageException.InvalidStorage();
        _storages.Add(storage);
    }

    public DateTime ChangeDateTime(DateTime dateTime)
    {
        DateTime = dateTime;
        return DateTime;
    }
}
