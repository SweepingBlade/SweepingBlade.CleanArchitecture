using System;

namespace SweepingBlade.Infrastructure.Domain.Dispatchers;

public sealed class EventDispatcher : IEventDispatcher
{
    private readonly IEventAggregator _eventAggregator;

    public EventDispatcher(IEventAggregator eventAggregator)
    {
        _eventAggregator = eventAggregator ?? throw new ArgumentNullException(nameof(eventAggregator));
    }

    public void Invoke<TEvent>(TEvent @event)
        where TEvent : IEvent<TEvent>
    {
        _eventAggregator.GetEventRegistration<TEvent>().Publish(@event);
    }
}