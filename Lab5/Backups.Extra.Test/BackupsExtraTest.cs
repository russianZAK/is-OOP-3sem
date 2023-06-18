using Backups.Entities;
using Backups.Extra.Entities;
using Backups.Extra.Models;
using Backups.Interfaces;
using Xunit;

namespace Backups.Extra.Test;

public class BackupsExtraTest
{
    [Fact]
    public void Backup_MergeCountLimit()
    {
        var newFileProvider = new FileProvider();
        var newFolderProvider = new FolderProvider();

        IObject newFile = newFileProvider.AddNewObject("test.txt", "test.txt");
        IObject newFolder = newFolderProvider.AddNewObject("newFolder");

        var newSystemRep = new ExtendedInMemoryRepository();
        var newAlgo = new SplitStorageAlgorithm();

        newSystemRep.AddNewObjects(newFolderProvider);
        newSystemRep.AddNewObjects(newFileProvider);
        var newExtendBackupTask = new ExtendedBackupTask("first", newSystemRep, newAlgo);
        var systemLogger = new FileLogger(true, "log.txt");
        var countRestorePointLimit = new RestorePointsCountLimit(2);
        var merge = new Merge(countRestorePointLimit);
        var decoratedBackupTask = new BackupTaskDecorator(newExtendBackupTask, systemLogger, merge);

        decoratedBackupTask.AddBackupObject(newFile);
        decoratedBackupTask.AddBackupObject(newFolder);
        decoratedBackupTask.BackUp();
        decoratedBackupTask.BackUp();
        decoratedBackupTask.DeleteBackupObject(newFolder);
        decoratedBackupTask.BackUp();
        Assert.NotEmpty(decoratedBackupTask.RestorePoints.ToList()[0].Storages);
        decoratedBackupTask.BackUp();
        Assert.Empty(decoratedBackupTask.RestorePoints.ToList()[0].Storages);
    }

    [Fact]
    public void Backup_MergeTimeLimit()
    {
        var newFileProvider = new FileProvider();
        var newFolderProvider = new FolderProvider();

        IObject newFile = newFileProvider.AddNewObject("test.txt", "test.txt");
        IObject newFolder = newFolderProvider.AddNewObject("newFolder");

        var newSystemRep = new ExtendedInMemoryRepository();
        var newAlgo = new SplitStorageAlgorithm();

        newSystemRep.AddNewObjects(newFolderProvider);
        newSystemRep.AddNewObjects(newFileProvider);
        var newExtendBackupTask = new ExtendedBackupTask("first", newSystemRep, newAlgo);
        var systemLogger = new FileLogger(true, "log.txt");
        var data = new DateTime(2015, 7, 20);
        var data2 = new DateTime(2015, 6, 20);
        var countRestorePointLimit = new RestorePointsDateLimit(data);
        var merge = new Merge(countRestorePointLimit);
        var decoratedBackupTask = new BackupTaskDecorator(newExtendBackupTask, systemLogger, merge);

        decoratedBackupTask.AddBackupObject(newFile);
        decoratedBackupTask.AddBackupObject(newFolder);
        decoratedBackupTask.BackUp();
        decoratedBackupTask.BackUp();
        decoratedBackupTask.DeleteBackupObject(newFolder);
        var allRestorePoints = decoratedBackupTask.RestorePoints.ToList();
        allRestorePoints.ForEach(restorePoint =>
        {
            decoratedBackupTask.DeleteRestorePoint(restorePoint);
            restorePoint.ChangeDateTime(data2);
        });

        allRestorePoints.ForEach(restorePoint =>
        {
            decoratedBackupTask.AddNewRestorePoint(restorePoint);
        });

        Assert.NotEmpty(decoratedBackupTask.RestorePoints.ToList()[0].Storages);
        decoratedBackupTask.BackUp();
        Assert.Empty(decoratedBackupTask.RestorePoints.ToList()[0].Storages);
    }

    [Fact]
    public void Recover()
    {
        var newFileProvider = new FileProvider();
        var newFolderProvider = new FolderProvider();

        IObject newFile = newFileProvider.AddNewObject("test.txt", "test.txt");
        IObject newFolder = newFolderProvider.AddNewObject("newFolder");

        var newSystemRep = new ExtendedInMemoryRepository();
        var secondSystemRep = new ExtendedInMemoryRepository();
        var newAlgo = new SplitStorageAlgorithm();

        newSystemRep.AddNewObjects(newFolderProvider);
        newSystemRep.AddNewObjects(newFileProvider);
        var newExtendBackupTask = new ExtendedBackupTask("first", newSystemRep, newAlgo);
        var systemLogger = new FileLogger(true, "log.txt");
        var countRestorePointLimit = new RestorePointsCountLimit(2);
        var merge = new Merge(countRestorePointLimit);
        var decoratedBackupTask = new BackupTaskDecorator(newExtendBackupTask, systemLogger, merge);

        decoratedBackupTask.AddBackupObject(newFile);
        decoratedBackupTask.AddBackupObject(newFolder);
        decoratedBackupTask.BackUp();
        decoratedBackupTask.DeleteBackupObject(newFolder);
        decoratedBackupTask.BackUp();
        decoratedBackupTask.BackUp();
        decoratedBackupTask.BackUp();
        decoratedBackupTask.RecoverRestorePointToDifferentLocation(decoratedBackupTask.RestorePoints.ToList()[1], secondSystemRep);

        Assert.Equal(2, secondSystemRep.GetRoot().Children.Count());
        Assert.Equal("test.txt", secondSystemRep.GetRoot().Children.ToList()[0].Name);
        Assert.Equal("newFolder", secondSystemRep.GetRoot().Children.ToList()[1].Name);
    }
}
