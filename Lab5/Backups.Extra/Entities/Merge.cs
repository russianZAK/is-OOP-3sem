using Backups.Entities;
using Backups.Extra.Exceptions;

namespace Backups.Extra.Entities;

public class Merge
{
    private readonly List<IRestorePointsLimit> _restorePointsLimit;
    private readonly List<RestorePoint> _allRestorePoints;
    private readonly int _maxStoragesWithSingleAlgorithm = 1;
    public Merge(IRestorePointsLimit restorePointsLimit)
    {
        if (restorePointsLimit == null) throw MergeException.InvalidRestorePointLimit();

        _restorePointsLimit = new List<IRestorePointsLimit>();
        _allRestorePoints = new List<RestorePoint>();

        _restorePointsLimit.Add(restorePointsLimit);
        IsUseAllLimits = false;
    }

    public Merge()
    {
        _allRestorePoints = new List<RestorePoint>();
        _restorePointsLimit = new List<IRestorePointsLimit>();
    }

    public bool IsUseAllLimits { get; private set; }

    public void AddAnotherRestorePointLimit(IRestorePointsLimit restorePointsLimit, bool isUseAllLimits)
    {
        if (restorePointsLimit == null) throw MergeException.InvalidRestorePointLimit();

        _restorePointsLimit.Add(restorePointsLimit);
        IsUseAllLimits = isUseAllLimits;
    }

    public List<RestorePoint> Merging(IEnumerable<RestorePoint> restorePoints)
    {
        if (restorePoints == null) throw MergeException.InvalidRestorePoint();

        var deletedStorages = new List<Storage>();
        var savedRestorePoints = new List<RestorePoint>();

        _allRestorePoints.Clear();
        _allRestorePoints.AddRange(restorePoints);

        var unfittedRestorePoints = new List<RestorePoint>();
        var newRestorePoints = new List<RestorePoint>();

        foreach (IRestorePointsLimit limit in _restorePointsLimit)
        {
            if (IsUseAllLimits)
            {
                unfittedRestorePoints.AddRange(limit.GetUnfittedRestorePoints(_allRestorePoints));
            }
            else
            {
                if (unfittedRestorePoints.Count() < limit.GetUnfittedRestorePoints(_allRestorePoints).ToList().Count())
                {
                    unfittedRestorePoints.AddRange(limit.GetUnfittedRestorePoints(_allRestorePoints));
                }
            }
        }

        if (_restorePointsLimit.Count() > 1 && IsUseAllLimits)
        {
            IEnumerable<RestorePoint> allUnfittedRestorePoints = unfittedRestorePoints.Distinct();
            unfittedRestorePoints.Clear();
            unfittedRestorePoints.AddRange(allUnfittedRestorePoints);
        }

        if (unfittedRestorePoints.Count() == _allRestorePoints.Count())
        {
            throw MergeException.InvalidMerging();
        }

        savedRestorePoints = restorePoints.Except(unfittedRestorePoints).ToList();

        var unfittedRestorePointsForDeleting = new List<RestorePoint>();
        unfittedRestorePoints.ForEach(restorePoint => unfittedRestorePointsForDeleting.Add(restorePoint));
        for (int i = 0; i < unfittedRestorePoints.Count() - 1; i++)
        {
            var allObjectsFromFirstRestorePoint = unfittedRestorePoints[i].BackupObjects.ToList();
            var allObjectsFromNextRestorePoint = unfittedRestorePoints[i + 1].BackupObjects.ToList();

            if (unfittedRestorePoints[i].Storages.Count == _maxStoragesWithSingleAlgorithm && unfittedRestorePoints[i + 1].Storages.Count == _maxStoragesWithSingleAlgorithm)
            {
                unfittedRestorePointsForDeleting[i].DeleteStorage(unfittedRestorePoints[i].Storages.First());
            }
            else
            {
                allObjectsFromFirstRestorePoint.ForEach(oldObject =>
                {
                    if (allObjectsFromNextRestorePoint.Contains(oldObject))
                    {
                        if (unfittedRestorePoints[i].Storages.Count != 0)
                        {
                            IEnumerable<Storage> selectedStorages = unfittedRestorePoints[i].Storages.ToList().Where(storage => storage.Objects.Contains(oldObject));
                            Storage firstSelectedStorage = selectedStorages.First();
                            unfittedRestorePointsForDeleting[i].DeleteStorage(firstSelectedStorage);
                        }
                    }
                    else
                    {
                        string pathToLastStorageInNextRestorePoint = unfittedRestorePoints[i + 1].Storages.Last().Path;
                        string newPath = Path.Combine(Path.GetDirectoryName(pathToLastStorageInNextRestorePoint) !, $"storage{unfittedRestorePoints[i + 1].Storages.Count()}.zip");

                        IEnumerable<Storage> selectedStorages = unfittedRestorePoints[i].Storages.ToList().Where(storage => storage.Objects.Contains(oldObject));
                        Storage firstSelectedStorage = selectedStorages.First();

                        var storageWithNewPath = new Storage(firstSelectedStorage.Objects, newPath);
                        unfittedRestorePointsForDeleting[i + 1].AddStorage(storageWithNewPath);
                    }
                });
            }
        }

        newRestorePoints.AddRange(unfittedRestorePointsForDeleting);
        newRestorePoints.AddRange(savedRestorePoints);

        return newRestorePoints;
    }
}
