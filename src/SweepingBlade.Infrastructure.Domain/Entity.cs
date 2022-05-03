using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace SweepingBlade.Infrastructure.Domain;

public abstract class Entity
{
    protected virtual void Invoke<TEvent>(TEvent @event)
        where TEvent : IEvent<TEvent>
    {
        EventManager.Current.Invoke(@event);
    }

    protected virtual bool SetProperty<T>(ref T storage, T value, PropertyChangedDelegate<T> action = null, [CallerMemberName] string propertyName = null)
    {
        if (!EqualityComparer<T>.Default.Equals(storage, value))
        {
            var oldValue = storage;
            storage = value;
            action?.Invoke(propertyName, oldValue, value);
            return true;
        }

        return false;
    }
}