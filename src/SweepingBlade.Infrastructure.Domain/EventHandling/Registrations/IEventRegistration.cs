using System;
using System.Threading;
using SweepingBlade.Infrastructure.Domain.Subscriptions;

namespace SweepingBlade.Infrastructure.Domain.EventHandling.Registrations
{
    public interface IEventRegistration
    {
        bool Contains(SubscriptionToken token);
        void Prune();
        void Unsubscribe(SubscriptionToken token);
        SynchronizationContext SynchronizationContext { get; }
    }

    public interface IEventRegistration<TEvent> : IEventRegistration
        where TEvent : IEvent
    {
        bool Contains(Action<TEvent> subscriber);
        void Publish(TEvent @event);
        SubscriptionToken Subscribe(Action<TEvent> action, bool keepSubscriberReferenceAlive = false, Predicate<TEvent> filter = null);
        void Unsubscribe(Action<TEvent> subscriber);
    }
}