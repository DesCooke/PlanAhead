namespace PlanAhead.Interfaces;

public interface INavigationContext
{
    void Set<T>(T item);

    T? Get<T>();

    bool Has<T>();

    void Clear();
}