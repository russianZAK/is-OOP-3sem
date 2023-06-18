using Backups.Entities;
using Backups.Exceptions;
using Backups.Interfaces;

namespace Backups.Models;

public class InMemoryRepository : IRepository
{
    private readonly List<IObject> _objects;
    private readonly List<Storage> _storages;
    private readonly Composite root;

    public InMemoryRepository()
    {
        _objects = new List<IObject>();
        _storages = new List<Storage>();

        root = new Composite("RootDirectory");
    }

    public string Path => root.Name;

    public IReadOnlyCollection<Storage> Storages => _storages;

    public IReadOnlyCollection<IObject> BackupObjects => _objects;

    public void AddNewObjects(IObjectProvider objectProvider)
    {
        if (objectProvider == null) throw InMemoryRepositoryException.InvalidProvider();

        foreach (IObject iobject in objectProvider.Objects)
        {
            root.Add(new Leaf(iobject.Path));
            _objects.Add(iobject);
        }
    }

    public void BackUp(IReadOnlyCollection<Storage> storages, string backupName)
    {
        if (storages == null) throw SystemRepositoryException.InvalidStorages();
        if (backupName == null) throw SystemRepositoryException.InvalidBackupName();

        _storages.AddRange(storages);

        var backupNameInFolder = new Composite(backupName);

        string? newRestorePointFolder = System.IO.Path.GetFileName(System.IO.Path.GetDirectoryName(storages.First().Path));

        if (newRestorePointFolder == null) throw InMemoryRepositoryException.InvalidRestorePoint();

        var restorePointFolder = new Composite(newRestorePointFolder);

        foreach (Storage storage in storages)
        {
            var storageFolder = new Composite(System.IO.Path.GetFileName(storage.Path));

            foreach (IObject storageObject in storage.Objects)
            {
                if (_objects.Contains(storageObject))
                {
                    storageFolder.Add(new Leaf(storageObject.Path));
                }
                else
                {
                    InMemoryRepositoryException.ObjectDoesntExist();
                }
            }

            if (!root.Contains(backupNameInFolder.Name))
            {
                restorePointFolder.Add(storageFolder);
                backupNameInFolder.Add(restorePointFolder);
                root.Add(backupNameInFolder);
            }
            else if (root.Contains(backupNameInFolder.Name) && !root.GetComponent(backupNameInFolder.Name).Contains(restorePointFolder.Name))
            {
                restorePointFolder.Add(storageFolder);
                IComponent backUp = root.GetComponent(backupNameInFolder.Name);
                backUp.Add(restorePointFolder);
            }
            else if (root.Contains(backupNameInFolder.Name) && root.GetComponent(backupNameInFolder.Name).Contains(restorePointFolder.Name))
            {
                IComponent backUp = root.GetComponent(backupNameInFolder.Name);
                IComponent restorePoint = backUp.GetComponent(restorePointFolder.Name);
                restorePoint.Add(storageFolder);
            }
        }
    }

    public IReadOnlyCollection<Storage> GetStorages()
    {
        return _storages;
    }

    public Composite GetRoot()
    {
        return root;
    }
}