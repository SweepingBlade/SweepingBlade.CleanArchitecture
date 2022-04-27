using SweepingBlade.Infrastructure.Domain.EventHandling.Registrations;

namespace SweepingBlade.Infrastructure.Domain
{
    public interface IEventAggregator
    {
        IEventRegistration<TEvent> GetEventRegistration<TEvent>() where TEvent : IEvent<TEvent>;
    }
}