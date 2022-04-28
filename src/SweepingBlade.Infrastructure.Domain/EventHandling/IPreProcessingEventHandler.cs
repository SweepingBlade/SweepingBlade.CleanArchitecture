namespace SweepingBlade.Infrastructure.Domain.EventHandling
{
    public interface IPreProcessingEventHandler<in TEvent>
        where TEvent : IEvent<TEvent>
    {
        void Handle(TEvent @event);
    }
}