using Backups.Entities;
using Backups.Exceptions;
using Backups.Extra.Entities;
using Backups.Extra.Exceptions;
using Backups.Interfaces;

namespace Backups.Extra.Models;

public class BackupTaskDecorator : IBackupTask
{
    private readonly IBackupTask _backUpTask;
    private readonly ILogger _logger;
    private readonly Merge _merge;

    public BackupTaskDecorator(ExtendedBackupTask extendBackupTask, ILogger logger, Merge merge)
    {
        if (extendBackupTask == null) throw BackupTaskDecoratorException.InvalidExtendedBackupTask();
        if (logger == null) throw BackupTaskDecoratorException.InvalidLogger();
        if (merge == null) throw BackupTaskDecoratorException.InvalidMerge();

        _backUpTask = extendBackupTask;
        _logger = logger;
        _merge = merge;
        ExtendedRepository = extendBackupTask.ExtendedRepository;
        SaveInfo();
    }

    public BackupTaskDecorator(IRestorePointsLimit restorePointsLimit, bool isUseAllLimits)
    {
        if (restorePointsLimit == null) throw BackupTaskDecoratorException.InvalidRestorePointLimit();

        BackupTaskDecorator oldBackupTask = ParseInfo();
        _backUpTask = oldBackupTask._backUpTask;
        ExtendedRepository = oldBackupTask.ExtendedRepository;
        _logger = oldBackupTask.Logger;
        _merge = oldBackupTask._merge;

        AddAnotherRestorePointLimit(restorePointsLimit, isUseAllLimits);
        SaveInfo();
    }

    public IExtendedRepository ExtendedRepository { get; }

    public IRepository Repository => _backUpTask.Repository;

    public ILogger Logger => _logger;

    public IStorageAlgorithm StorageAlgorithm => _backUpTask.StorageAlgorithm;

    public IReadOnlyCollection<IObject> BackupObjects => _backUpTask.BackupObjects;

    public IReadOnlyCollection<RestorePoint> RestorePoints => _backUpTask.RestorePoints;

    public string BackupName => _backUpTask.BackupName;

    public void AddAnotherRestorePointLimit(IRestorePointsLimit restorePointsLimit, bool isUseAllLimits)
    {
        if (restorePointsLimit == null) throw BackupTaskDecoratorException.InvalidRestorePointLimit();
        _merge.AddAnotherRestorePointLimit(restorePointsLimit, isUseAllLimits);

        _logger.Logging($"Added new restore point limit - {restorePointsLimit.GetType().Name}");
    }

    public void AddBackupObject(IObject newObject)
    {
        if (newObject == null) throw BackupTaskDecoratorException.InvalidObject();
        _backUpTask.AddBackupObject(newObject);

        _logger.Logging($"Added new object - {newObject.Path}");
    }

    public void AddNewRestorePoint(RestorePoint restorePoint)
    {
        if (restorePoint == null) throw BackupTaskDecoratorException.InvalidRestorePoint();
        _backUpTask.AddNewRestorePoint(restorePoint);

        _logger.Logging($"Added new restore point - {restorePoint.Id}");

        _logger.Logging("With storages:");

        foreach (Storage storage in restorePoint.Storages)
        {
            _logger.Logging($"{storage.Path}");
        }
    }

    public void BackUp()
    {
        _backUpTask.BackUp();

        _logger.Logging($"Added new restore point {_backUpTask.RestorePoints.Last().Id}");

        RepeatedMethods();
    }

    public void ChangeAlgorithm(IStorageAlgorithm storageAlgorithm)
    {
        if (storageAlgorithm == null) throw BackupTaskException.InvalidStorageAlgorithm();

        _backUpTask.ChangeAlgorithm(storageAlgorithm);

        _logger.Logging($"Changed storage algorithm to - {storageAlgorithm.GetType().Name}");

        SaveInfo();
    }

    public void ChangeRepository(IRepository repository)
    {
        if (repository == null) throw BackupTaskException.InvalidRepository();

        _backUpTask.ChangeRepository(repository);

        _logger.Logging($"Changed repository to - {repository.GetType().Name} - {repository.Path}");

        SaveInfo();
    }

    public void DeleteBackupObject(IObject backupObject)
    {
        if (backupObject == null) throw BackupTaskDecoratorException.InvalidObject();

        _backUpTask.DeleteBackupObject(backupObject);

        _logger.Logging($"Removed object - {backupObject.Path}");

        SaveInfo();
    }

    public void DeleteRestorePoint(RestorePoint restorePoint)
    {
        if (restorePoint == null) throw BackupTaskDecoratorException.InvalidRestorePoint();

        _backUpTask.DeleteRestorePoint(restorePoint);

        _logger.Logging($"Removed restore point - {restorePoint.Id}");

        _logger.Logging("With storages:");

        foreach (Storage storage in restorePoint.Storages)
        {
            _logger.Logging($"{storage.Path}");
        }

        SaveInfo();
    }

    public void RecoverRestorePointToDifferentLocation(RestorePoint restorePoint, IExtendedRepository repository)
    {
        if (restorePoint == null) throw BackupTaskDecoratorException.InvalidRestorePoint();
        if (repository == null) throw BackupTaskException.InvalidRepository();

        ExtendedRepository.RecoverRestorePointToDifferentLocation(restorePoint, repository);

        _logger.Logging($"Recovered restore point - {restorePoint.Id} - to {repository.GetType().Name} - {repository.Path}");

        _logger.Logging("With storages:");

        foreach (Storage storage in restorePoint.Storages)
        {
            _logger.Logging($"{storage.Path}");
        }

        SaveInfo();
    }

    public void RecoverRestorePointToOriginalLocation(RestorePoint restorePoint)
    {
        if (restorePoint == null) throw BackupTaskDecoratorException.InvalidRestorePoint();

        ExtendedRepository.RecoverRestorePointToOriginalLocation(restorePoint);

        _logger.Logging($"Recovered restore point - {restorePoint.Id} - to original location");

        _logger.Logging("With storages:");

        foreach (Storage storage in restorePoint.Storages)
        {
            _logger.Logging($"{storage.Path}");
        }

        SaveInfo();
    }

    private void RepeatedMethods()
    {
        Merging();
        SaveInfo();
    }

    private void Merge()
    {
        Merging();
    }

    private void Merging()
    {
        var oldRestorePoint = new List<RestorePoint>();
        oldRestorePoint.AddRange(_backUpTask.RestorePoints);

        var newStorages = new List<Storage>();
        List<RestorePoint> newRestorePoints = _merge.Merging(_backUpTask.RestorePoints);

        for (int i = 0; i < oldRestorePoint.Count; i++)
        {
            if (newRestorePoints[i].Storages.Count == 0 || newRestorePoints[i].Storages.Count < newRestorePoints[i].BackupObjects.Count)
            {
                _logger.Logging($"Merged restore point - {oldRestorePoint[i].Id}");

                foreach (IObject oldObject in newRestorePoints[i].BackupObjects)
                {
                    _logger.Logging($"Merged {oldObject.Path}");
                }
            }
        }

        string backupPath = System.IO.Path.Combine(Repository.Path, _backUpTask.BackupName);
        RestorePoint restorePointForDeleteing = oldRestorePoint.First();

        ExtendedRepository.DeleteRestorePoint(restorePointForDeleteing, backupPath);

        foreach (RestorePoint restorePoint in oldRestorePoint)
        {
            _backUpTask.DeleteRestorePoint(restorePoint);
        }

        foreach (RestorePoint newRestorePoint in newRestorePoints)
        {
            foreach (Storage storage in newRestorePoint.Storages)
            {
                newStorages.Add(storage);
            }

            _backUpTask.AddNewRestorePoint(newRestorePoint);
        }

        Repository.BackUp(newStorages, _backUpTask.BackupName);
    }

    private BackupTaskDecorator ParseInfo()
    {
        string[] data = System.IO.File.ReadAllLines("config.txt");
        var parser = new BackupTaskParser();

        BackupTaskDecorator newBackupTask = parser.Parse(data);
        return newBackupTask;
    }

    private void SaveInfo()
    {
        var saver = new BackupTaskSaver();
        saver.Save(this);

        _logger.Logging("Saved backup task information");
    }
}
