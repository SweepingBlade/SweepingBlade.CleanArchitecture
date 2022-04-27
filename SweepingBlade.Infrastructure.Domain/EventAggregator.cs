using System;
using System.Collections.Generic;
using System.Threading;
using SweepingBlade.Infrastructure.Domain.EventHandling.Registrations;
using SweepingBlade.Infrastructure.Domain.EventHandling.Resolvers;

namespace SweepingBlade.Infrastructure.Domain
{
    public class EventAggregator : IEventAggregator
    {
        private readonly Dictionary<Type, IEventRegistration> _events;
        private readonly IPostProcessingEventHandlerResolver _postProcessingEventHandlerResolver;
        private readonly IPreProcessingEventHandlerResolver _preProcessingEventHandlerResolver;
        private readonly SynchronizationContext _synchronizationContext;

        public EventAggregator(IPreProcessingEventHandlerResolver preProcessingEventHandlerResolver, IPostProcessingEventHandlerResolver postProcessingEventHandlerResolver, SynchronizationContext synchronizationContext)
        {
            _preProcessingEventHandlerResolver = preProcessingEventHandlerResolver ?? throw new ArgumentNullException(nameof(preProcessingEventHandlerResolver));
            _postProcessingEventHandlerResolver = postProcessingEventHandlerResolver ?? throw new ArgumentNullException(nameof(postProcessingEventHandlerResolver));
            _synchronizationContext = synchronizationContext ?? throw new ArgumentNullException(nameof(synchronizationContext));
            _events = new Dictionary<Type, IEventRegistration>();
        }

        public virtual IEventRegistration<TEvent> GetEventRegistration<TEvent>() where TEvent : IEvent<TEvent>
        {
            lock (_events)
            {
                var type = typeof(TEvent);
                if (_events.TryGetValue(type, out var existingEvent))
                {
                    return (IEventRegistration<TEvent>)existingEvent;
                }

                var eventRegistration = new EventRegistration<TEvent>(_preProcessingEventHandlerResolver, _postProcessingEventHandlerResolver, _synchronizationContext);
                _events[type] = eventRegistration;
                return eventRegistration;
            }
        }
    }
}