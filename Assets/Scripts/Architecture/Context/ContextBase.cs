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

            var targetLocator = IsCommonContext ? CommonServiceLocator : _contextServiceLocator;
            InjectInstances(targetLocator);
            InitializeBehaviours(targetLocator);
        }

        protected abstract void InjectInstances(IServiceLocator serviceLocator);
        protected virtual void InitializeBehaviours(IServiceLocator serviceLocator){}

        private void OnDestroy()
        {
            _contextServiceLocator?.Dispose();
        }
    }
}