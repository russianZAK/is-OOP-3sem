namespace Backups.Models;

public interface IComponent
{
    public string Name { get; }
    public void Add(IComponent component);
    public void Remove(IComponent component);
    public bool Contains(string componentName);
    public IComponent GetComponent(string componentName);
}
