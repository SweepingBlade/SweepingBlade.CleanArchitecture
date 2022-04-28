using System.Collections.Generic;

namespace SweepingBlade.Infrastructure.Domain.EventHandling.Resolvers
{
    public interface IPostProcessingEventHandlerResolver
    {
        IEnumerable<IPostProcessingEventHandler<TEvent>> Resolve<TEvent>() where TEvent : IEvent<TEvent>;
    }
}