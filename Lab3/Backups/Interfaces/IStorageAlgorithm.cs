using Backups.Entities;

namespace Backups.Interfaces;

public interface IStorageAlgorithm
{
    public List<Storage> StartAlgorithm(IReadOnlyCollection<IObject> objects, string backupPath, int restorePointId);
}
