namespace SweepingBlade.Infrastructure.Domain.EventHandling
{
    public interface IPostProcessingEventHandler<in TEvent>
        where TEvent : IEvent<TEvent>
    {
        void Handle(TEvent @event);
    }
}