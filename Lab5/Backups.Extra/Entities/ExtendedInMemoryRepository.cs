using Backups.Entities;
using Backups.Extra.Exceptions;
using Backups.Interfaces;
using Backups.Models;

namespace Backups.Extra.Entities;

public class ExtendedInMemoryRepository : InMemoryRepository, IExtendedRepository
{
    public ExtendedInMemoryRepository()
        : base()
    {
    }

    public void RecoverRestorePointToDifferentLocation(RestorePoint restorePoint, IExtendedRepository repository)
    {
        if (restorePoint == null) throw ExtendedRepositoryException.InvalidRestorePoint();
        if (repository == null) throw ExtendedRepositoryException.InvalidRepository();

        if (!(repository is ExtendedInMemoryRepository)) throw ExtendedRepositoryException.InvalidRepository();

        var extendedRepository = (ExtendedInMemoryRepository)repository;

        foreach (Storage storage in restorePoint.Storages)
        {
            foreach (IObject oldObject in storage.Objects)
            {
                var newLeaf = new Leaf(oldObject.Path);
                extendedRepository.GetRoot().Add(newLeaf);
            }
        }
    }

    public void RecoverRestorePointToOriginalLocation(RestorePoint restorePoint)
    {
        if (restorePoint == null) throw ExtendedRepositoryException.InvalidRestorePoint();

        foreach (Storage storage in restorePoint.Storages)
        {
            foreach (IObject oldObject in storage.Objects)
            {
                var newLeaf = new Leaf(oldObject.Path);
                if (GetRoot().Contains(oldObject.Path))
                {
                    GetRoot().Remove(newLeaf);
                    GetRoot().Add(newLeaf);
                }
                else
                {
                    GetRoot().Add(newLeaf);
                }
            }
        }
    }

    public void DeleteRestorePoint(RestorePoint restorePoint, string backupName)
    {
        if (restorePoint == null) throw ExtendedRepositoryException.InvalidRestorePoint();
        if (backupName == null) throw ExtendedRepositoryException.InvalidBackupName();

        var backupNameInFolder = new Composite(backupName);

        GetRoot().Remove(backupNameInFolder);
    }

    public IRepository ReturnRepository()
    {
        return this;
    }
}
