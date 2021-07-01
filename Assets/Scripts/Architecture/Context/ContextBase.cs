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
            if(!IsCommonContext)
            {
                _contextServiceLocator = new ServiceLocator.ServiceLocator();
                _contextServiceLocator.SetParent(CommonServiceLocator);
            }

            var targetLocator = IsCommonContext ? CommonServiceLocator : _contextServiceLocator;
            InjectInstances(targetLocator);
            InitializeMonoBehaviours(targetLocator);
        }

        protected abstract void InjectInstances(IServiceLocator serviceLocator);
        protected virtual void InitializeMonoBehaviours(IServiceLocator serviceLocator){}

        private void OnDestroy()
        {
            _contextServiceLocator?.Dispose();
        }
    }
}