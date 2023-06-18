using Backups.Exceptions;

namespace Backups.Models;

public class Leaf : IComponent
{
    private string name;

    public Leaf(string name)
    {
        this.name = name;
    }

    public string Name => name;

    public void Add(IComponent component)
    {
        throw ComponentException.InvalidOperationException();
    }

    public void Remove(IComponent component)
    {
        throw ComponentException.InvalidOperationException();
    }

    public bool Contains(string componentName)
    {
        throw ComponentException.InvalidOperationException();
    }

    public IComponent GetComponent(string componentName)
    {
        throw ComponentException.InvalidOperationException();
    }
}
