using Backups.Exceptions;

namespace Backups.Models;

public class Composite : IComponent
{
    private List<IComponent> _children = new List<IComponent>();
    private string name;

    public Composite(string name)
    {
        if (name == null) throw CompositeException.InvalidComponentName();
        this.name = name;
    }

    public IReadOnlyCollection<IComponent> Children => _children;

    public string Name => name;

    public void Add(IComponent component)
    {
        if (component == null) throw ComponentException.InvalidComponent();

        _children.Add(component);
    }

    public void Remove(IComponent component)
    {
        if (component == null) throw ComponentException.InvalidComponent();

        _children.Remove(component);
    }

    public bool Contains(string componentName)
    {
        bool contains = false;

        if (componentName == null) throw CompositeException.InvalidComponentName();

        contains = _children.Any(component => component.Name == componentName);

        return contains;
    }

    public IComponent GetComponent(string componentName)
    {
        if (componentName == null) throw CompositeException.InvalidComponentName();

        IComponent? component = _children.Where(component => component.Name == componentName).FirstOrDefault();

        if (component == null) throw ComponentException.InvalidComponent();

        return component;
    }
}
