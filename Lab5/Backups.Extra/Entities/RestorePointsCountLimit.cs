using Backups.Entities;
using Backups.Extra.Exceptions;

namespace Backups.Extra.Entities;

public class RestorePointsCountLimit : IRestorePointsLimit
{
    private readonly List<RestorePoint> _unfittedRestorePoints;
    public RestorePointsCountLimit(int maximumLimitOfRestorePoint)
    {
        if (maximumLimitOfRestorePoint < 0) throw RestorePointCountLimitException.InvalidLimit(maximumLimitOfRestorePoint);
        _unfittedRestorePoints = new List<RestorePoint>();
        MaximumLimitOfRestorePoint = maximumLimitOfRestorePoint;
    }

    public int MaximumLimitOfRestorePoint { get; }

    public IEnumerable<RestorePoint> GetUnfittedRestorePoints(IEnumerable<RestorePoint> restorePoints)
    {
        if (restorePoints == null) throw BackupTaskDecoratorException.InvalidRestorePoint();

        _unfittedRestorePoints.Clear();
        for (int i = restorePoints.ToList().Count() - 1; i >= 0; i--)
        {
            if (restorePoints.ToList().Count() - 1 - i >= MaximumLimitOfRestorePoint)
            {
                if (restorePoints.ToList()[i].Storages.Count != 0) _unfittedRestorePoints.Add(restorePoints.ToList()[i]);
            }
        }

        _unfittedRestorePoints.Reverse();
        return _unfittedRestorePoints;
    }
}
