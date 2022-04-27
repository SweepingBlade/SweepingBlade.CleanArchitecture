using System.Collections.Generic;

namespace SweepingBlade.Infrastructure.Domain.EventHandling.Resolvers
{
    public interface IPreProcessingEventHandlerResolver
    {
        IEnumerable<IPreProcessingEventHandler<TEvent>> Resolve<TEvent>() where TEvent : IEvent<TEvent>;
    }
}