using Backups.Exceptions;
using Backups.Interfaces;

namespace Backups.Entities;

public class FolderProvider : IObjectProvider
{
    private readonly List<IObject> _objects;
    private readonly List<string> _objectsPaths;

    public FolderProvider()
    {
        _objects = new List<IObject>();
        _objectsPaths = new List<string>();
    }

    public IReadOnlyCollection<IObject> Objects => _objects;
    public IReadOnlyCollection<string> ObjectsPaths => _objectsPaths;

    public IObject AddNewObject(string path)
    {
        if (path == null) throw FolderException.InvalidPath();

        _objectsPaths.Add(path);
        _objects.Add(new Folder(path));

        return _objects.Last();
    }
}
