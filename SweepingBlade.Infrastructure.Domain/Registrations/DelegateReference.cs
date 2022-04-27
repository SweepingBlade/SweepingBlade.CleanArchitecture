using System;
using System.Reflection;

namespace SweepingBlade.Infrastructure.Domain.Registrations
{
    public class DelegateReference : IDelegateReference
    {
        private readonly Delegate _delegate;
        private readonly Type _delegateType;
        private readonly MethodInfo _method;
        private readonly WeakReference _weakReference;

        public DelegateReference(Delegate @delegate, bool keepReferenceAlive)
        {
            if (@delegate is null)
            {
                throw new ArgumentNullException(nameof(@delegate));
            }

            if (keepReferenceAlive)
            {
                _delegate = @delegate;
            }
            else
            {
                _weakReference = new WeakReference(@delegate.Target);
                _method = @delegate.GetMethodInfo();
                _delegateType = @delegate.GetType();
            }
        }

        public virtual Delegate Target => _delegate ?? TryGetDelegate();

        public virtual bool TargetEquals(Delegate @delegate)
        {
            if (_delegate is not null)
            {
                return _delegate == @delegate;
            }

            if (@delegate is null)
            {
                return !_method.IsStatic && !_weakReference.IsAlive;
            }

            return _weakReference.Target == @delegate.Target && Equals(_method, @delegate.GetMethodInfo());
        }

        protected virtual Delegate TryGetDelegate()
        {
            if (_method.IsStatic)
            {
                return _method.CreateDelegate(_delegateType, null);
            }

            var target = _weakReference.Target;
            return target is not null ? _method.CreateDelegate(_delegateType, target) : null;
        }
    }
}