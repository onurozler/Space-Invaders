using Architecture.ServiceLocator;
using Core.Controllers;
using Core.Views.Menu;
using UnityEngine;

namespace Architecture.Context
{
    public class MenuContext : ContextBase
    {
        [SerializeField] private MenuView menuView;
        
        protected override void InjectInstances(IServiceLocator serviceLocator)
        {
            serviceLocator.Add<IMenuView>(menuView);
            serviceLocator.Add(new MenuController(serviceLocator));
        }
    }
}