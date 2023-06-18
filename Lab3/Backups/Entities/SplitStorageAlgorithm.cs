using Backups.Exceptions;
using Backups.Interfaces;

namespace Backups.Entities;

public class SplitStorageAlgorithm : IStorageAlgorithm
{
    public SplitStorageAlgorithm() { }

    public List<Storage> StartAlgorithm(IReadOnlyCollection<IObject> objects, string backupPath, int restorePointId)
    {
        if (objects == null) throw SplitStorageAlgorithmException.InvalidObjects();
        if (backupPath == null) throw SplitStorageAlgorithmException.InvalidBackupPath();
        if (restorePointId < 0) throw SplitStorageAlgorithmException.InvalidId(restorePointId);

        int countOfStorages = 0;
        var splitStorage = new List<Storage>();

        foreach (IObject backupObject in objects)
        {
            var listForObjects = new List<IObject>();
            listForObjects.Add(backupObject);
            string storagePath = Path.Combine(backupPath, $"RestorePoint{restorePointId}", $"storage{countOfStorages}.zip");
            countOfStorages++;
            var newStorage = new Storage(listForObjects, storagePath);
            splitStorage.Add(newStorage);
        }

        return splitStorage;
    }
}
