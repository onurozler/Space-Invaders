using Architecture.ServiceLocator;
using Core.Controllers.Impl;
using Core.Views.Game;
using UnityEngine;

namespace Architecture.Context
{
    public class GameContext : ContextBase
    {
        [SerializeField] private GameView gameView;
        
        protected override void InjectInstances(IServiceLocator serviceLocator)
        {
            serviceLocator.Add<IGameView>(gameView);
            serviceLocator.Add(new GameController());
        }
    }
}