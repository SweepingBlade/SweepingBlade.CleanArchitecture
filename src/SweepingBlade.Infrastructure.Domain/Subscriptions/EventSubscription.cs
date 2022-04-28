using System;
using System.Globalization;
using SweepingBlade.Infrastructure.Domain.Registrations;

namespace SweepingBlade.Infrastructure.Domain.Subscriptions
{
    public class EventSubscription : IEventSubscription
    {
        private readonly IDelegateReference _actionReference;

        public Action Action => (Action)_actionReference.Target;

        public EventSubscription(IDelegateReference actionReference)
        {
            if (actionReference is null)
            {
                throw new ArgumentNullException(nameof(actionReference));
            }

            if (actionReference.Target is not System.Action)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "Invalid Delegate Reference Type Exception"), nameof(actionReference));
            }

            _actionReference = actionReference;
        }

        public virtual SubscriptionToken SubscriptionToken { get; set; }

        public virtual Action GetExecutionStrategy()
        {
            return () => InvokeAction(Action);
        }

        protected virtual void InvokeAction(Action action)
        {
            action?.Invoke();
        }
    }

    public class EventSubscription<TEvent> : IEventSubscription<TEvent>
        where TEvent : IEvent<TEvent>
    {
        private readonly IDelegateReference _actionReference;
        private readonly IDelegateReference _filterReference;

        public virtual Action<TEvent> Action => (Action<TEvent>)_actionReference.Target;

        public virtual Predicate<TEvent> Filter => (Predicate<TEvent>)_filterReference.Target;

        public EventSubscription(IDelegateReference actionReference, IDelegateReference filterReference)
        {
            if (actionReference is null)
            {
                throw new ArgumentNullException(nameof(actionReference));
            }

            if (actionReference.Target is not Action<TEvent>)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "Invalid Delegate Reference Type Exception"), nameof(actionReference));
            }

            if (filterReference is null)
            {
                throw new ArgumentNullException(nameof(filterReference));
            }

            if (filterReference.Target is not Predicate<TEvent>)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "Invalid Delegate Reference Type Exception"), nameof(filterReference));
            }

            _actionReference = actionReference;
            _filterReference = filterReference;
        }

        public virtual SubscriptionToken SubscriptionToken { get; set; }

        public virtual Action<TEvent> GetExecutionStrategy()
        {
            var action = Action;
            var filter = Filter;
            if (action is not null && filter is not null)
            {
                return payload =>
                {
                    if (filter(payload))
                    {
                        InvokeAction(action, payload);
                    }
                };
            }

            return null;
        }

        public virtual void InvokeAction(Action<TEvent> action, TEvent argument)
        {
            if (action is null) throw new ArgumentNullException(nameof(action));

            action(argument);
        }
    }
}