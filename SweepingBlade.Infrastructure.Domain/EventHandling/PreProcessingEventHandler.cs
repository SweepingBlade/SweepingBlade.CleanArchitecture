namespace SweepingBlade.Infrastructure.Domain.EventHandling
{
    public abstract class PreProcessingEventHandler<TEvent> : IPreProcessingEventHandler<TEvent>
        where TEvent : IEvent<TEvent>, new()
    {
        public abstract void Handle(TEvent @event);
    }
}