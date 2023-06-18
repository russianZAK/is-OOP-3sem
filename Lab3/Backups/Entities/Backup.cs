using Backups.Exceptions;

namespace Backups.Entities;

public class Backup
{
    private readonly List<RestorePoint> _restorePoints;

    public Backup()
    {
        _restorePoints = new List<RestorePoint>();
    }

    public IReadOnlyCollection<RestorePoint> RestorePoints => _restorePoints;

    public void AddNewRestorePoint(RestorePoint restorePoint)
    {
        if (restorePoint == null) throw BackupException.InvalidRestorePoint();

        _restorePoints.Add(restorePoint);
    }

    public void DeleteRestorePoint(RestorePoint restorePoint)
    {
        if (restorePoint == null) throw BackupException.InvalidRestorePoint();
        if (!_restorePoints.Contains(restorePoint)) throw BackupException.InvalidRestorePoint();

        _restorePoints.Remove(restorePoint);
    }
}
