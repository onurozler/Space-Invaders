using Architecture.ServiceLocator;

namespace Core.Managers
{
    public class ShieldsManager : MonoPoolManagerBase
    {
        public void Initialize(IServiceLocator serviceLocator)
        {
            InitializePool(CreateShields);
        }

        private void CreateShields()
        {
            
        }
    }
}