namespace SweepingBlade.Infrastructure.Domain;

public interface IEvent
{
}

public interface IEvent<TEvent> : IEvent
    where TEvent : IEvent<TEvent>
{
}