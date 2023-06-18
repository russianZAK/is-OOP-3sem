using Backups.Extra.Entities;
using Backups.Extra.Exceptions;
using Backups.Interfaces;
using Backups.Service;

namespace Backups.Extra.Models;

public class ExtendedBackupTask : BackupTask, IBackupTask
{
    public ExtendedBackupTask(string backupName, IExtendedRepository repository, IStorageAlgorithm storageAlgorithm)
        : base(backupName, repository, storageAlgorithm)
    {
        if (backupName == null) throw ExtendedBackupTaskException.InvalidBackupName();
        if (repository == null) throw ExtendedBackupTaskException.InvalidRepository();
        if (storageAlgorithm == null) throw ExtendedBackupTaskException.InvalidStorageAlgorithm();

        ExtendedRepository = repository;
    }

    public IExtendedRepository ExtendedRepository { get; }
}
