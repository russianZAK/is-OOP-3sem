using Backups.Exceptions;
using Backups.Interfaces;

namespace Backups.Entities;

public class Folder : IObject
{
    public Folder(string path)
    {
        if (path == null) throw FolderException.InvalidPath();
        Path = path;
    }

    public string Path { get; }
}
