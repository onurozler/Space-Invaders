using Architecture.ServiceLocator;
using UnityEngine;

namespace Architecture.Context
{
    public abstract class ContextBase : MonoBehaviour
    {
        protected virtual bool IsCommonContext => false;

        private static readonly IServiceLocator CommonServiceLocator = new ServiceLocator.ServiceLocator();
        private IServiceLocator _contextServiceLocator;

        protected virtual void Awake()
        {
            if (IsCommonContext)
            {
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                _contextServiceLocator = new ServiceLocator.ServiceLocator();
                _contextServiceLocator.SetParent(CommonServiceLocator);
            }
            
            InjectInstances(IsCommonContext ? CommonServiceLocator : _contextServiceLocator);
        }

        protected abstract void InjectInstances(IServiceLocator serviceLocator);

        private void OnDestroy()
        {
            _contextServiceLocator?.Dispose();
        }
    }
}