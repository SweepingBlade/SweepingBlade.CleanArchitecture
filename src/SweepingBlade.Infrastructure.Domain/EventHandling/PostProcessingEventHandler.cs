namespace SweepingBlade.Infrastructure.Domain.EventHandling
{
    public abstract class PostProcessingEventHandler<TEvent> : IPostProcessingEventHandler<TEvent>
        where TEvent : IEvent<TEvent>, new()
    {
        public abstract void Handle(TEvent @event);
    }
}