using Backups.Entities;
using Backups.Extra.Models;
using Backups.Interfaces;
using Backups.Models;

namespace Backups.Extra.Entities;

public class BackupTaskParser
{
    public BackupTaskParser() { }

    public BackupTaskDecorator Parse(string[] data)
    {
        string backupTaskName = string.Empty;
        string repositoryPath = string.Empty;

        IExtendedRepository? repository = null;
        IStorageAlgorithm? storageAlgorithm = null;

        var fileProvider = new FileProvider();
        var folderProvider = new FolderProvider();

        var objectsInRepository = new List<IObject>();
        var newObjectProvider = new List<IObjectProvider>();

        var objectsInBackupTask = new List<IObject>();
        var restorePoints = new List<RestorePoint>();

        var restorePointStorages = new List<Storage>();
        var objectsStorages = new List<IObject>();

        var objectsrestorePoint = new List<IObject>();

        int restorePointId = 0;
        string storagePath = string.Empty;

        ILogger? logger = null;
        DateTime restorepointDateTime = DateTime.Now;

        foreach (string str in data)
        {
            string[] splitData = str.Split();
            switch (splitData[0])
            {
                case "BackupTaskName":
                    backupTaskName = splitData[1];
                    break;

                case "RepositoryPath":
                    repositoryPath = splitData[1];
                    break;

                case "ObjectInRepository":
                    newObjectProvider.Add(GetObjectProvider(splitData[1], splitData[2]) !);
                    break;

                case "Repository":
                    repository = GetRepository(splitData[1], repositoryPath) !;
                    newObjectProvider.ForEach(iObjectProvider =>
                        {
                            repository!.AddNewObjects(iObjectProvider!);
                        });
                    break;

                case "StorageAlgorithm":
                    storageAlgorithm = GetStorageAlgorithm(splitData[1]) !;
                    break;

                case "ObjectInBackupTask":
                    objectsInBackupTask.Add(GetIObject(splitData[1], splitData[2]) !);
                    break;

                case "RestorepointId":
                    if (restorePoints.Count() != 0 || Convert.ToInt32(splitData[1]) != 0)
                    {
                        var restorePoint = new RestorePoint(objectsrestorePoint!, restorePointId, restorePointStorages);
                        restorePoint.ChangeDateTime(restorepointDateTime);
                        restorePointStorages.Clear();
                        objectsrestorePoint.Clear();
                        restorePoints.Add(restorePoint);
                    }

                    restorePointId = Convert.ToInt32(splitData[1]);
                    break;

                case "CountOfRestorePoints":
                    var lastRestorePoint = new RestorePoint(objectsrestorePoint!, restorePointId, restorePointStorages);
                    lastRestorePoint.ChangeDateTime(restorepointDateTime);
                    restorePoints.Add(lastRestorePoint);
                    break;

                case "RestorepointDateTime":
                    restorepointDateTime = Convert.ToDateTime(splitData[1]);
                    break;

                case "RestorePointObject":
                    objectsrestorePoint.Add(GetIObject(splitData[1], splitData[2]) !);
                    break;

                case "Storage":
                    storagePath = splitData[1];
                    var newStorage = new Storage(objectsStorages!, storagePath);
                    restorePointStorages.Add(newStorage);

                    break;

                case "StorageObject":
                    objectsStorages.Add(GetIObject(splitData[1], splitData[2]) !);
                    break;

                case "ILogger":
                    if (splitData[1] == "ConsoleLogger")
                    {
                        logger = new ConsoleLogger(Convert.ToBoolean(splitData[2]));
                    }
                    else if (splitData[1] == "FileLogger")
                    {
                        logger = new FileLogger(Convert.ToBoolean(splitData[2]), "log.txt");
                    }

                    break;
            }
        }

        var backupTask = new ExtendedBackupTask(backupTaskName, new RepositoryLoggerDecorator(repository!, logger!), storageAlgorithm!);

        foreach (RestorePoint restorePoint in restorePoints)
        {
            backupTask.AddNewRestorePoint(restorePoint);
        }

        foreach (IObject? newObject in objectsInBackupTask)
        {
            backupTask.AddBackupObject(newObject!);
        }

        var newDecoratedBackupTask = new BackupTaskDecorator(backupTask, logger!, new Merge());

        return newDecoratedBackupTask;
    }

    private IObjectProvider? GetObjectProvider(string typeOfObject, string objectPath)
    {
        IObjectProvider objectProvider;

        if (typeOfObject == "File")
        {
            objectProvider = new FileProvider();
            objectProvider.AddNewObject(objectPath);
            return objectProvider;
        }

        if (typeOfObject == "Folder")
        {
            objectProvider = new FolderProvider();
            objectProvider.AddNewObject(objectPath);
            return objectProvider;
        }

        return null;
    }

    private IExtendedRepository? GetRepository(string typeOfRepository, string repositoryPath)
    {
        IExtendedRepository repository;
        if (typeOfRepository == "ExtendedSystemRepository")
        {
            repository = new ExtendedSystemRepository(repositoryPath);
            return repository;
        }
        else if (typeOfRepository == "ExtendedInMemoryRepository")
        {
            repository = new ExtendedInMemoryRepository();
            return repository;
        }

        return null;
    }

    private IStorageAlgorithm? GetStorageAlgorithm(string typeOfStorageAlgorithm)
    {
        IStorageAlgorithm storageAlgorithm;

        if (typeOfStorageAlgorithm == "SplitStorageAlgorithm")
        {
            storageAlgorithm = new SplitStorageAlgorithm();
            return storageAlgorithm;
        }
        else if (typeOfStorageAlgorithm == "SingleStorageAlgorithm")
        {
            storageAlgorithm = new SingleStorageAlgorithm();
            return storageAlgorithm;
        }

        return null;
    }

    private IObject? GetIObject(string typeOfObject, string path)
    {
        IObject newObject;
        Console.WriteLine(path);
        if (typeOfObject == "File")
        {
            newObject = new Backups.Entities.File(path, Path.GetFileName(path));
            return newObject;
        }
        else if (typeOfObject == "Folder")
        {
            newObject = new Backups.Entities.Folder(path);
            return newObject;
        }

        return null;
    }
}
