using Architecture.ServiceLocator;
using Core.Behaviours;
using UnityEngine;

namespace Core.Managers
{
    public class ShieldsManager : MonoBehaviour
    {
        [SerializeField] private ShieldBehaviour[] shields;
        
        public void Initialize(IServiceLocator serviceLocator)
        {
            InitializeShields();
        }

        private void InitializeShields()
        {
            foreach (var shield in shields)
            {
                shield.Initialize();
            }
        }
    }
}