using Backups.Entities;
using Backups.Extra.Exceptions;
using Backups.Interfaces;
namespace Backups.Extra.Entities;

public class RepositoryLoggerDecorator : IExtendedRepository
{
    private readonly IExtendedRepository _repository;
    private readonly ILogger _logger;

    public RepositoryLoggerDecorator(IExtendedRepository repository, ILogger logger)
    {
        if (repository == null) throw RepositoryLoggerDecoratorException.InvalidRepository();
        if (logger == null) throw RepositoryLoggerDecoratorException.InvalidLogger();

        _repository = repository;
        _logger = logger;
    }

    public string Path => _repository.Path;

    public IReadOnlyCollection<Storage> Storages => _repository.Storages;

    public IReadOnlyCollection<IObject> BackupObjects => _repository.BackupObjects;

    public void AddNewObjects(IObjectProvider objectProvider)
    {
        if (objectProvider == null) throw ExtendedRepositoryException.InvalidProvider();
        _repository.AddNewObjects(objectProvider);

        foreach (IObject newObject in objectProvider.Objects)
        {
            _logger.Logging($"Added object {newObject.Path}");
        }
    }

    public void BackUp(IReadOnlyCollection<Storage> storages, string backupName)
    {
        if (storages == null) throw ExtendedRepositoryException.InvalidStorages();
        if (backupName == null) throw ExtendedRepositoryException.InvalidBackupName();

        _repository.BackUp(storages, backupName);

        foreach (Storage storage in storages)
        {
            _logger.Logging($"Created storage {storage.Path}");
        }
    }

    public void DeleteRestorePoint(RestorePoint restorePoint, string backupName)
    {
        if (restorePoint == null) throw ExtendedRepositoryException.InvalidRestorePoint();
        if (backupName == null) throw ExtendedRepositoryException.InvalidBackupName();
        _repository.DeleteRestorePoint(restorePoint, backupName);
    }

    public ILogger GetLogger()
    {
        return _logger;
    }

    public void RecoverRestorePointToDifferentLocation(RestorePoint restorePoint, IExtendedRepository repository)
    {
        if (restorePoint == null) throw ExtendedRepositoryException.InvalidRestorePoint();
        if (repository == null) throw ExtendedRepositoryException.InvalidRepository();

        _repository.RecoverRestorePointToDifferentLocation(restorePoint, repository);
    }

    public void RecoverRestorePointToOriginalLocation(RestorePoint restorePoint)
    {
        if (restorePoint == null) throw ExtendedRepositoryException.InvalidRestorePoint();

        _repository.RecoverRestorePointToOriginalLocation(restorePoint);
    }

    public IRepository ReturnRepository()
    {
        return _repository;
    }
}
