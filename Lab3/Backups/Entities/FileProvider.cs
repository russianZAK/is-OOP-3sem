using Backups.Exceptions;
using Backups.Interfaces;

namespace Backups.Entities;

public class FileProvider : IObjectProvider
{
    private readonly List<IObject> _objects;
    private readonly List<string> _objectsPaths;

    public FileProvider()
    {
        _objects = new List<IObject>();
        _objectsPaths = new List<string>();
    }

    public IReadOnlyCollection<IObject> Objects => _objects;
    public IReadOnlyCollection<string> ObjectsPaths => _objectsPaths;

    public IObject AddNewObject(string path, string name)
    {
        if (path == null) throw FileException.InvalidPath();
        if (name == null) throw FileException.InvalidName();

        _objectsPaths.Add(path);
        _objects.Add(new File(path, name));

        return _objects.Last();
    }

    public IObject AddNewObject(string path)
    {
        if (path == null) throw FileException.InvalidPath();
        _objectsPaths.Add(path);
        _objects.Add(new File(path, Path.GetFileName(path)));

        return _objects.Last();
    }
}
