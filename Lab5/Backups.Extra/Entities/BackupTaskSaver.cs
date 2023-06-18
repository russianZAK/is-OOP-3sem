using Backups.Entities;
using Backups.Extra.Exceptions;
using Backups.Extra.Models;
using Backups.Interfaces;

namespace Backups.Extra.Entities;

public class BackupTaskSaver
{
    public BackupTaskSaver() { }

    public void Save(BackupTaskDecorator backupTask)
    {
        if (backupTask == null) throw BackupTaskDecoratorException.InvalidBackupTask();

        using (var streamWriter = new StreamWriter(new FileStream("config.txt", FileMode.Create, FileAccess.Write)))
        {
            streamWriter.WriteLine($"BackupTaskName {backupTask.BackupName}");
            streamWriter.WriteLine($"ILogger {backupTask.Logger.GetType().Name} {backupTask.Logger.IsUseDateTime}");
            streamWriter.WriteLine($"RepositoryPath {backupTask.Repository.Path}");

            foreach (IObject backUpObject in backupTask.Repository.BackupObjects)
            {
                streamWriter.WriteLine($"ObjectInRepository {backUpObject.GetType().Name} {backUpObject.Path}");
            }

            streamWriter.WriteLine($"Repository {backupTask.ExtendedRepository.ReturnRepository().GetType().Name}");

            streamWriter.WriteLine($"StorageAlgorithm {backupTask.StorageAlgorithm.GetType().Name}");
            foreach (IObject backUpObject in backupTask.BackupObjects)
            {
                streamWriter.WriteLine($"ObjectInBackupTask {backUpObject.GetType().Name} {backUpObject.Path}");
            }

            foreach (RestorePoint restorePoint in backupTask.RestorePoints)
            {
                streamWriter.WriteLine($"RestorepointId {restorePoint.Id}");
                streamWriter.WriteLine($"RestorepointDateTime {restorePoint.DateTime}");
                streamWriter.WriteLine($"CountofBackupObjects {restorePoint.BackupObjects.Count()}");
                foreach (IObject backUpObject in restorePoint.BackupObjects)
                {
                    streamWriter.WriteLine($"RestorePointObject {backUpObject.GetType().Name} {backUpObject.Path}");
                }

                foreach (Storage storage in restorePoint.Storages)
                {
                    foreach (IObject storageObject in storage.Objects)
                    {
                        streamWriter.WriteLine($"StorageObject {storageObject.GetType().Name} {storageObject.Path}");
                    }

                    streamWriter.WriteLine($"Storage {storage.Path}");
                }
            }

            streamWriter.WriteLine($"CountOfRestorePoints {backupTask.RestorePoints.Count()}");
        }
    }
}
