using System;

namespace SweepingBlade.Infrastructure.Domain.Registrations
{
    public interface IDelegateReference
    {
        Delegate Target { get; }
    }
}