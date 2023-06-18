using Backups.Entities;
using Backups.Interfaces;
using Backups.Models;
using Backups.Service;
using Xunit;

namespace Backups.Test;

public class BackupTaskTest
{
    [Fact]
    public void Backup()
    {
        var newFileProvider = new FileProvider();
        var newFolderProvider = new FolderProvider();

        IObject newFile = newFileProvider.AddNewObject("test.txt", "test.txt");
        IObject newFolder = newFolderProvider.AddNewObject("newFolder");

        var newSystemRep = new InMemoryRepository();
        var newAlgo = new SplitStorageAlgorithm();

        newSystemRep.AddNewObjects(newFolderProvider);
        newSystemRep.AddNewObjects(newFileProvider);
        var newBackupTask = new BackupTask("first", newSystemRep, newAlgo);
        newBackupTask.AddBackupObject(newFile);
        newBackupTask.AddBackupObject(newFolder);
        newBackupTask.BackUp();
        newBackupTask.DeleteBackupObject(newFolder);
        newBackupTask.BackUp();

        Assert.Equal(3, newSystemRep.GetStorages().Count());
        Assert.Equal(2, newBackupTask.RestorePoints.Count());
    }
}
