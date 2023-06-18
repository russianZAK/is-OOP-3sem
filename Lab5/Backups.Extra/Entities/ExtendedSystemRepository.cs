using System.IO.Compression;
using Backups.Entities;
using Backups.Extra.Exceptions;
using Backups.Interfaces;

namespace Backups.Extra.Entities;

public class ExtendedSystemRepository : SystemRepository, IExtendedRepository
{
    public ExtendedSystemRepository(string path)
        : base(path)
    {
        if (path == null) throw ExtendedRepositoryException.InvalidPath();
    }

    public void RecoverRestorePointToDifferentLocation(RestorePoint restorePoint, IExtendedRepository repository)
    {
        if (restorePoint == null) throw ExtendedRepositoryException.InvalidRestorePoint();
        if (repository == null) throw ExtendedRepositoryException.InvalidRepository();

        if (!(repository is ExtendedSystemRepository)) throw ExtendedRepositoryException.InvalidRepository();

        foreach (Storage storage in restorePoint.Storages)
        {
            using (ZipArchive archive = ZipFile.Open(storage.Path, ZipArchiveMode.Update))
            {
                archive.ExtractToDirectory(repository.Path);
            }
        }
    }

    public void RecoverRestorePointToOriginalLocation(RestorePoint restorePoint)
    {
        if (restorePoint == null) throw ExtendedRepositoryException.InvalidRestorePoint();

        foreach (Storage storage in restorePoint.Storages)
        {
            using (ZipArchive archive = ZipFile.Open(storage.Path, ZipArchiveMode.Update))
            {
                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    foreach (IObject oldObject in storage.Objects)
                    {
                        if (entry.FullName.Equals(System.IO.Path.GetFileName(oldObject.Path), StringComparison.OrdinalIgnoreCase))
                        {
                            string destinationPath = oldObject.Path;
                            entry.ExtractToFile(destinationPath);
                        }
                    }
                }
            }
        }
    }

    public void DeleteRestorePoint(RestorePoint restorePoint, string backupName)
    {
        if (restorePoint == null) throw ExtendedRepositoryException.InvalidRestorePoint();
        if (backupName == null) throw ExtendedRepositoryException.InvalidBackupName();

        string backupPath = System.IO.Path.Combine(Path, backupName);

        Directory.Delete(backupPath, true);
    }

    public IRepository ReturnRepository()
    {
        return this;
    }
}
