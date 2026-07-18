using PlanAhead.core.Interfaces;

namespace PlanAhead.Services;

public class NavigationContext : INavigationContext
{
    private readonly Dictionary<Type, object> _items = new();

    public void Set<T>(T item)
    {
        _items[typeof(T)] = item!;
    }

    public T? Get<T>()
    {
        if (_items.TryGetValue(typeof(T), out var value))
        {
            _items.Remove(typeof(T)); // Consume once
            return (T)value;
        }

        return default;
    }

    public bool Has<T>()
    {
        return _items.ContainsKey(typeof(T));
    }

    public void Clear()
    {
        _items.Clear();
    }
}