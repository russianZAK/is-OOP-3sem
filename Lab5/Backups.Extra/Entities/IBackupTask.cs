using Backups.Entities;
using Backups.Interfaces;

namespace Backups.Extra.Entities;

public interface IBackupTask
{
    IRepository Repository { get; }
    IStorageAlgorithm StorageAlgorithm { get; }
    IReadOnlyCollection<IObject> BackupObjects { get; }
    IReadOnlyCollection<RestorePoint> RestorePoints { get; }
    string BackupName { get; }
    void BackUp();
    void ChangeAlgorithm(IStorageAlgorithm storageAlgorithm);
    void ChangeRepository(IRepository repository);
    void AddBackupObject(IObject newObject);
    void DeleteBackupObject(IObject backupObject);
    void AddNewRestorePoint(RestorePoint restorePoint);
    void DeleteRestorePoint(RestorePoint restorePoint);
}
