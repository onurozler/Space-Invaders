using System.Collections.Generic;
using Architecture.ServiceLocator;
using Core.Behaviours.Player;
using Core.Controllers.Impl;
using Core.Managers;
using Core.Models.Enemy;
using Core.Models.Game;
using Core.Models.Game.Input;
using Core.Models.Player;
using Core.Models.Ship;
using Core.Views.Game;
using Helpers.Scene;
using Helpers.Timing;
using UnityEngine;

namespace Architecture.Context
{
    public class GameContext : ContextBase
    {
        [Header("Resources")]
        [SerializeField] private ShipAssetData[] shipAssetDatas;
        [SerializeField] private GameAssetData gameAssetData;
        
        [Header("Others")]
        [SerializeField] private PlayerBaseBehaviour player;
        [SerializeField] private CoroutineTimingManager coroutineTimingManager;
        [SerializeField] private ShipsManager shipsManager;
        [SerializeField] private ShieldsManager shieldsManager;
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
            serviceLocator.Add<ITimingManager>(coroutineTimingManager);
            serviceLocator.Add(new EnemyFormationData());
            serviceLocator.Add(new PlayerData());
            serviceLocator.Add(gameAssetData);
            serviceLocator.Add<ISceneStateHandler>(sceneStateHandler);
            
            serviceLocator.Add<IGameView>(gameView);
            serviceLocator.Add(new GameController(serviceLocator));
            
            serviceLocator.Add(bulletManager);
            serviceLocator.Add(enemiesManager);
            
            serviceLocator.Add(new EnemiesController(serviceLocator));
            
            serviceLocator.Add<IList<ShipAssetData>>(shipAssetDatas);
        }

        protected override void InitializeMonoBehaviours(IServiceLocator serviceLocator)
        {
            player.Initialize(serviceLocator);
            enemiesManager.Initialize(serviceLocator);
            bulletManager.Initialize(serviceLocator);
            shieldsManager.Initialize(serviceLocator);
            shipsManager.Initialize(serviceLocator);
        }
    }
}