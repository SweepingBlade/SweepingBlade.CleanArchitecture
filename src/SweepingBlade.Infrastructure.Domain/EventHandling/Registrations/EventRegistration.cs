using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using SweepingBlade.Infrastructure.Domain.EventHandling.Resolvers;
using SweepingBlade.Infrastructure.Domain.Registrations;
using SweepingBlade.Infrastructure.Domain.Subscriptions;

namespace SweepingBlade.Infrastructure.Domain.EventHandling.Registrations
{
    public class EventRegistration<TEvent> : IEventRegistration<TEvent>
        where TEvent : IEvent<TEvent>
    {
        private readonly IPostProcessingEventHandlerResolver _postProcessingEventHandlerResolver;
        private readonly IPreProcessingEventHandlerResolver _preProcessingEventHandlerResolver;
        private readonly List<IEventSubscription<TEvent>> _subscriptions;

        public EventRegistration(IPreProcessingEventHandlerResolver preProcessingEventHandlerResolver, IPostProcessingEventHandlerResolver postProcessingEventHandlerResolver)
        {
            _preProcessingEventHandlerResolver = preProcessingEventHandlerResolver ?? throw new ArgumentNullException(nameof(preProcessingEventHandlerResolver));
            _postProcessingEventHandlerResolver = postProcessingEventHandlerResolver ?? throw new ArgumentNullException(nameof(postProcessingEventHandlerResolver));

            _subscriptions = new List<IEventSubscription<TEvent>>();
        }

        public virtual bool Contains(SubscriptionToken token)
        {
            lock (_subscriptions)
            {
                var subscription = _subscriptions.FirstOrDefault(evt => ReferenceEquals(evt.SubscriptionToken, token));
                return subscription is not null;
            }
        }

        public virtual void Prune()
        {
            lock (_subscriptions)
            {
                for (var i = _subscriptions.Count - 1; i >= 0; i--)
                {
                    if (_subscriptions[i].GetExecutionStrategy() is null)
                    {
                        _subscriptions.RemoveAt(i);
                    }
                }
            }
        }

        public virtual void Unsubscribe(SubscriptionToken token)
        {
            lock (_subscriptions)
            {
                var subscription = _subscriptions.FirstOrDefault(evt => ReferenceEquals(evt.SubscriptionToken, token));
                if (subscription is not null)
                {
                    _subscriptions.Remove(subscription);
                }
            }
        }

        public virtual bool Contains(Action<TEvent> subscriber)
        {
            IEventSubscription<TEvent> eventSubscription;
            lock (_subscriptions)
            {
                eventSubscription = _subscriptions.Cast<EventSubscription<TEvent>>().FirstOrDefault(subscription => subscription.Action == subscriber);
            }

            return eventSubscription is not null;
        }

        public virtual void Publish(TEvent @event)
        {
            var executionStrategies = PruneAndReturnStrategies();
            var preProcessingHandlerInstances = _preProcessingEventHandlerResolver.Resolve<TEvent>();
            foreach (var preProcessingHandlerInstance in preProcessingHandlerInstances)
            {
                preProcessingHandlerInstance.Handle(@event);
            }

            foreach (var executionStrategy in executionStrategies)
            {
                executionStrategy(@event);
            }

            var postProcessingHandlerInstances = _postProcessingEventHandlerResolver.Resolve<TEvent>();
            foreach (var postProcessingHandlerInstance in postProcessingHandlerInstances)
            {
                postProcessingHandlerInstance.Handle(@event);
            }
        }

        public virtual SubscriptionToken Subscribe(Action<TEvent> action, bool keepSubscriberReferenceAlive = false, Predicate<TEvent> filter = null)
        {
            IDelegateReference actionReference = new DelegateReference(action, keepSubscriberReferenceAlive);
            IDelegateReference filterReference;
            if (filter is not null)
            {
                filterReference = new DelegateReference(filter, keepSubscriberReferenceAlive);
            }
            else
            {
                filterReference = new DelegateReference(new Predicate<TEvent>(delegate { return true; }), true);
            }

            var subscription = new EventSubscription<TEvent>(actionReference, filterReference);

            return Subscribe(subscription);
        }

        public virtual void Unsubscribe(Action<TEvent> subscriber)
        {
            lock (_subscriptions)
            {
                IEventSubscription<TEvent> eventSubscription = _subscriptions.Cast<EventSubscription<TEvent>>().FirstOrDefault(evt => evt.Action == subscriber);
                if (eventSubscription is not null)
                {
                    _subscriptions.Remove(eventSubscription);
                }
            }
        }

        protected virtual IEnumerable<Action<TEvent>> PruneAndReturnStrategies()
        {
            var returnList = new List<Action<TEvent>>();

            lock (_subscriptions)
            {
                for (var i = _subscriptions.Count - 1; i >= 0; i--)
                {
                    var listItem = _subscriptions[i].GetExecutionStrategy();

                    if (listItem is null)
                    {
                        _subscriptions.RemoveAt(i);
                    }
                    else
                    {
                        returnList.Add(listItem);
                    }
                }
            }

            return returnList;
        }

        protected virtual SubscriptionToken Subscribe(IEventSubscription<TEvent> eventSubscription)
        {
            if (eventSubscription is null) throw new ArgumentNullException(nameof(eventSubscription));

            eventSubscription.SubscriptionToken = new SubscriptionToken(Unsubscribe);

            lock (_subscriptions)
            {
                _subscriptions.Add(eventSubscription);
            }

            return eventSubscription.SubscriptionToken;
        }
    }
}