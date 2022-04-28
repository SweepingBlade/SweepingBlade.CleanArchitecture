using System.Collections;
using System.Collections.Generic;

namespace SweepingBlade.Infrastructure.Domain;

public abstract class Collection<T> : Entity, IList<T>
{
    private readonly List<T> _list;

    public T this[int index]
    {
        get => _list[index];
        set => _list[index] = value;
    }

    protected Collection()
    {
        _list = new List<T>();
    }

    public int Count => _list.Count;
    public bool IsReadOnly => false;

    public void Add(T item)
    {
        _list.Add(item);
        OnAddedItem(item);
    }

    public void Clear()
    {
        _list.Clear();
        OnCleared();
    }

    public bool Contains(T item)
    {
        return _list.Contains(item);
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
        _list.CopyTo(array, arrayIndex);
    }

    public bool Remove(T item)
    {
        var result = _list.Remove(item);

        if (result)
        {
            OnRemoved(item);
        }

        return result;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public IEnumerator<T> GetEnumerator()
    {
        return _list.GetEnumerator();
    }

    public int IndexOf(T item)
    {
        return _list.IndexOf(item);
    }

    public void Insert(int index, T item)
    {
        _list.Insert(index, item);
        OnInserted(index, item);
    }

    public void RemoveAt(int index)
    {
        var item = _list[index];
        Remove(item);
    }

    protected virtual void OnAddedItem(T item)
    {
    }

    protected virtual void OnCleared()
    {
    }

    protected virtual void OnInserted(int index, T item)
    {
    }

    protected virtual void OnMoved(T item, int oldIndex, int newIndex)
    {
    }

    protected virtual void OnRemoved(T item)
    {
    }
}