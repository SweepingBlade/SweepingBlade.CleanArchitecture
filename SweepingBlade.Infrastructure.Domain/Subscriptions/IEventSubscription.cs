using System;

namespace SweepingBlade.Infrastructure.Domain.Subscriptions
{
    public interface IEventSubscription
    {
        Action GetExecutionStrategy();
        SubscriptionToken SubscriptionToken { get; set; }
    }

    public interface IEventSubscription<in TEvent>
        where TEvent : IEvent<TEvent>
    {
        Action<TEvent> GetExecutionStrategy();
        SubscriptionToken SubscriptionToken { get; set; }
    }
}