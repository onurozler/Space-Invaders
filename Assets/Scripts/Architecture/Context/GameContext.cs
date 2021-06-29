using Architecture.ServiceLocator;
using Core.Behaviours.Player;
using Core.Controllers.Impl;
using Core.Managers;
using Core.Managers.Enemy;
using Core.Models.Enemy;
using Core.Models.Game.Input;
using Core.Views.Game;
using Helpers.Scene;
using Helpers.Timing;
using UnityEngine;

namespace Architecture.Context
{
    public class GameContext : ContextBase
    {
        [SerializeField] private PlayerBaseBehaviour player;
        [SerializeField] private CoroutineTimingManager coroutineTimingManager;
        [SerializeField] private BulletManager bulletManager;
        [SerializeField] private EnemiesManager enemiesManager;
        [SerializeField] private SceneStateHandler sceneStateHandler;
        [SerializeField] private GameView gameView;
        
        protected override void InjectInstances(IServiceLocator serviceLocator)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            serviceLocator.Add<IGameInputData>(new MobileInputData());
#else
            serviceLocator.Add<IGameInputData>(new KeyboardInputData());
#endif
            
            serviceLocator.Add<ISceneStateHandler>(sceneStateHandler);
            
            serviceLocator.Add<IGameView>(gameView);
            serviceLocator.Add(new GameController(serviceLocator));
            
            serviceLocator.Add<ITimingManager>(coroutineTimingManager);
            serviceLocator.Add(bulletManager);
            serviceLocator.Add(enemiesManager);
            
            serviceLocator.Add(new EnemyFormationData());
            serviceLocator.Add(new EnemiesController(serviceLocator));
        }

        protected override void InitializeBehaviours(IServiceLocator serviceLocator)
        {
            player.Initialize(serviceLocator);
            enemiesManager.Initialize(serviceLocator);
            bulletManager.Initialize(serviceLocator);
        }
    }
}