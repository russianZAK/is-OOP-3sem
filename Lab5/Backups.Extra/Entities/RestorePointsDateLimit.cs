using Backups.Entities;
using Backups.Extra.Exceptions;

namespace Backups.Extra.Entities;

public class RestorePointsDateLimit : IRestorePointsLimit
{
    private readonly List<RestorePoint> _unfittedRestorePoints;
    public RestorePointsDateLimit(DateTime dateLimit)
    {
        _unfittedRestorePoints = new List<RestorePoint>();
        DateLimit = dateLimit;
    }

    public DateTime DateLimit { get; }

    public IEnumerable<RestorePoint> GetUnfittedRestorePoints(IEnumerable<RestorePoint> restorePoints)
    {
        if (restorePoints == null) throw BackupTaskDecoratorException.InvalidRestorePoint();

        _unfittedRestorePoints.Clear();
        foreach (RestorePoint restorePoint in restorePoints)
        {
            if (DateLimit > restorePoint.DateTime)
            {
                if (restorePoint.Storages.Count != 0) _unfittedRestorePoints.Add(restorePoint);
            }
        }

        return _unfittedRestorePoints;
    }
}
