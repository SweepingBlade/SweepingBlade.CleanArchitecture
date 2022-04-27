namespace SweepingBlade.Infrastructure.Domain.Dispatchers;

public interface IEventDispatcher
{
    void Invoke<TEvent>(TEvent @event)
        where TEvent : IEvent<TEvent>;
}