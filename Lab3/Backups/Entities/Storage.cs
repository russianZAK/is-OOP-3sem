using Backups.Exceptions;
using Backups.Interfaces;

namespace Backups.Entities;

public class Storage
{
    private readonly List<IObject> _objects;

    public Storage(IReadOnlyCollection<IObject> objects, string path)
    {
        if (objects == null) throw StorageException.InvalidObjects();
        if (path == null) throw StorageException.InvalidPath();
        _objects = objects.ToList();
        Path = path;
    }

    public string Path { get; }
    public IReadOnlyCollection<IObject> Objects => _objects;
}
