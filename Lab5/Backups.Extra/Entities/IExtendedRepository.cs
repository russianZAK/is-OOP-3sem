using Backups.Entities;
using Backups.Interfaces;

namespace Backups.Extra.Entities;

public interface IExtendedRepository : IRepository
{
    void DeleteRestorePoint(RestorePoint restorePoint, string backupName);
    void RecoverRestorePointToDifferentLocation(RestorePoint restorePoint, IExtendedRepository repository);
    void RecoverRestorePointToOriginalLocation(RestorePoint restorePoint);
    IRepository ReturnRepository();
}
