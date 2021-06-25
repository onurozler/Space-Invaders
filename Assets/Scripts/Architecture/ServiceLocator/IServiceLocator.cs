using System;

namespace Architecture.ServiceLocator
{
    public interface IServiceLocator : IDisposable
    {
        void Add<T>(T instance);
        T Get<T>();
        void SetParent(IServiceLocator serviceLocator);
    }
}