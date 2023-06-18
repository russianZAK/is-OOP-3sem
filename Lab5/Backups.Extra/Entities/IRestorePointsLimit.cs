using Backups.Entities;
namespace Backups.Extra.Entities;

public interface IRestorePointsLimit
{
    IEnumerable<RestorePoint> GetUnfittedRestorePoints(IEnumerable<RestorePoint> restorePoints);
}
