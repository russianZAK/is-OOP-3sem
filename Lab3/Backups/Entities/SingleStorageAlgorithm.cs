using Backups.Entities;
using Backups.Exceptions;
using Backups.Interfaces;

namespace Backups.Models;

public class SingleStorageAlgorithm : IStorageAlgorithm
{
    public SingleStorageAlgorithm() { }

    public List<Storage> StartAlgorithm(IReadOnlyCollection<IObject> objects, string backupPath, int restorePointId)
    {
        if (objects == null) throw SingleStorageAlgorithmException.InvalidObjects();
        if (backupPath == null) throw SingleStorageAlgorithmException.InvalidBackupPath();
        if (restorePointId < 0) throw SingleStorageAlgorithmException.InvalidId(restorePointId);

        int countOfStorages = 0;
        var singleStorage = new List<Storage>();

        string storagePath = Path.Combine(backupPath, $"RestorePoint{restorePointId}", $"storage{countOfStorages}.zip");

        var storage = new Storage(objects, storagePath);
        singleStorage.Add(storage);

        return singleStorage;
    }
}
