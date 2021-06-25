using System;
using System.Collections.Generic;

namespace Architecture.ServiceLocator
{
    public class ServiceLocator : IServiceLocator
    {
        private readonly Dictionary<Type, object> _instancePairs;
        private IServiceLocator _parentLocator;

        public ServiceLocator()
        {
            _instancePairs = new Dictionary<Type, object>();
        }

        public void Add<T>(T instance)
        {
            _instancePairs[typeof(T)] = instance;
        }

        public T Get<T>()
        {
            if(_instancePairs.TryGetValue(typeof(T), out var value))
            {
                return (T) value;
            }

            return _parentLocator == null ? default : _parentLocator.Get<T>();
        }

        public void SetParent(IServiceLocator serviceLocator)
        {
            _parentLocator = serviceLocator;
        }

        public void Dispose()
        {
            foreach (var instancePair in _instancePairs)
            {
                var value = instancePair.Value;
                if (value is IDisposable disposableValue)
                {
                    disposableValue.Dispose();
                }
            }
            
            _instancePairs.Clear();
        }
    }
}