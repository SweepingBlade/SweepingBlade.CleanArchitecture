using System;
using System.ComponentModel;
using SweepingBlade.Infrastructure.Domain.Dispatchers;

namespace SweepingBlade.Infrastructure.Domain;

[EditorBrowsable(EditorBrowsableState.Never)]
public static class EventManager
{
    private static IEventDispatcher _current;
    private static Lazy<IEventDispatcher> _lazy;

    public static IEventDispatcher Current => _current ??= _lazy?.Value;

    [EditorBrowsable(EditorBrowsableState.Never)]
    public static void Reset()
    {
        _current = null;
        _lazy = null;
    }

    public static void Set(Func<IEventDispatcher> factory)
    {
        _lazy = new Lazy<IEventDispatcher>(factory);
    }
}