using Backups.Exceptions;
using Backups.Interfaces;

namespace Backups.Entities;

public class File : IObject
{
    public File(string path, string name)
    {
        if (path == null) throw FileException.InvalidPath();
        if (name == null) throw FileException.InvalidName();
        Path = path;
        Name = name;
    }

    public string Path { get; }

    public string Name { get; }
}
