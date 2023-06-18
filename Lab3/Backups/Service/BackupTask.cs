using Backups.Entities;
using Backups.Exceptions;
using Backups.Interfaces;

namespace Backups.Service;

public class BackupTask
{
    private readonly List<IObject> _backupObjects;
    private readonly Backup _backup;

    public BackupTask(string backupName, IRepository repository, IStorageAlgorithm storageAlgorithm)
    {
        if (backupName == null) throw BackupTaskException.InvalidName();
        if (repository == null) throw BackupTaskException.InvalidRepository();
        if (storageAlgorithm == null) throw BackupTaskException.InvalidStorageAlgorithm();

        _backup = new Backup();
        _backupObjects = new List<IObject>();
        BackupName = backupName;
        Repository = repository;
        StorageAlgorithm = storageAlgorithm;
    }

    public IRepository Repository { get; private set; }
    public IStorageAlgorithm StorageAlgorithm { get; private set; }
    public IReadOnlyCollection<IObject> BackupObjects => _backupObjects;
    public IReadOnlyCollection<RestorePoint> RestorePoints => _backup.RestorePoints;
    public string BackupName { get; }

    public void BackUp()
    {
        string pathToStorages = Path.Combine(Repository.Path, BackupName);

        List<Storage> storages = StorageAlgorithm.StartAlgorithm(_backupObjects, pathToStorages, _backup.RestorePoints.Count());
        Repository.BackUp(storages, BackupName);

        var newRestorePoint = new RestorePoint(_backupObjects, _backup.RestorePoints.Count(), storages);
        _backup.AddNewRestorePoint(newRestorePoint);
    }

    public void ChangeAlgorithm(IStorageAlgorithm storageAlgorithm)
    {
        if (storageAlgorithm == null) throw BackupTaskException.InvalidStorageAlgorithm();

        StorageAlgorithm = storageAlgorithm;
    }

    public void ChangeRepository(IRepository repository)
    {
        if (repository == null) throw BackupTaskException.InvalidRepository();
        Repository = repository;
    }

    public void AddBackupObject(IObject newObject)
    {
        if (newObject == null) throw BackupTaskException.InvalidObject();

        _backupObjects.Add(newObject);
    }

    public void DeleteBackupObject(IObject backupObject)
    {
        if (backupObject == null) throw BackupTaskException.InvalidObject();

        if (!_backupObjects.Contains(backupObject)) throw BackupTaskException.ObjectsDoesntExist();

        _backupObjects.Remove(backupObject);
    }

    public void AddNewRestorePoint(RestorePoint restorePoint)
    {
        if (restorePoint == null) throw BackupException.InvalidRestorePoint();

        _backup.AddNewRestorePoint(restorePoint);
    }

    public void DeleteRestorePoint(RestorePoint restorePoint)
    {
        if (restorePoint == null) throw BackupException.InvalidRestorePoint();
        if (!_backup.RestorePoints.Contains(restorePoint)) throw BackupTaskException.RestorePointDoesntExist();

        _backup.DeleteRestorePoint(restorePoint);
    }
}
