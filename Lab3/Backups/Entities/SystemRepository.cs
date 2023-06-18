using System.IO.Compression;
using Backups.Exceptions;
using Backups.Interfaces;

namespace Backups.Entities;

public class SystemRepository : IRepository
{
    private readonly List<IObject> _backupObjects;
    private readonly List<Storage> _storages;

    public SystemRepository(string path)
    {
        if (path == null) throw SystemRepositoryException.InvalidPath();

        _backupObjects = new List<IObject>();
        _storages = new List<Storage>();
        Path = path;
    }

    public string Path { get; }

    public IReadOnlyCollection<Storage> Storages => _storages;

    public IReadOnlyCollection<IObject> BackupObjects => _backupObjects;

    public void AddNewObjects(IObjectProvider objectProvider)
    {
        if (objectProvider == null) throw SystemRepositoryException.InvalidProvider();
        _backupObjects.AddRange(objectProvider.Objects);
    }

    public void BackUp(IReadOnlyCollection<Storage> storages, string backupName)
    {
        if (storages == null) throw SystemRepositoryException.InvalidStorages();
        if (backupName == null) throw SystemRepositoryException.InvalidBackupName();

        _storages.AddRange(storages);
        int countOfStorages = 0;
        string backupPath = System.IO.Path.Combine(Path, backupName);
        Directory.CreateDirectory(backupPath);

        foreach (Storage storage in storages)
        {
            string? restorePointPath = System.IO.Path.GetDirectoryName(storage.Path);

            if (restorePointPath == null) throw SystemRepositoryException.InvalidRestorePoint();

            Directory.CreateDirectory(restorePointPath);

            countOfStorages++;

            using (ZipArchive zipArchive = ZipFile.Open(storage.Path, ZipArchiveMode.Create))
            {
                foreach (IObject storageObject in storage.Objects)
                {
                    if (Directory.Exists(storageObject.Path))
                    {
                        ZipFile.CreateFromDirectory(storageObject.Path, storageObject.Path + ".zip");
                        zipArchive.CreateEntryFromFile(storageObject.Path + ".zip", new DirectoryInfo(storageObject.Path).Name + ".zip");

                        System.IO.File.Delete(storageObject.Path + ".zip");
                    }
                    else if (System.IO.File.Exists(storageObject.Path))
                    {
                        zipArchive.CreateEntryFromFile(storageObject.Path, System.IO.Path.GetFileName(storageObject.Path));
                    }
                }
            }
        }
    }
}
