using System.Collections.Generic;
using Architecture.ServiceLocator;
using Core.Behaviours;
using Core.Controllers;
using Core.Managers;
using Core.Models.Enemy;
using Core.Models.Game;
using Core.Models.Game.Input;
using Core.Models.Player;
using Core.Models.Ship;
using Core.Views.Game;
using Core.Views.Leaderboard;
using Helpers.Scene;
using UnityEngine;

namespace Architecture.Context
{
    public class GameContext : ContextBase
    {
        [Header("Resources")]
        [SerializeField] private ShipAssetData[] shipAssetDatas;
        [SerializeField] private GameAssetData gameAssetData;
        
        [Header("Others")]
        [SerializeField] private PlayerBehaviour player;
        [SerializeField] private ShipsManager shipsManager;
        [SerializeField] private ShieldsManager shieldsManager;
        [SerializeField] private BulletManager bulletManager;
        [SerializeField] private EnemiesManager enemiesManager;
        [SerializeField] private SceneStateHandler sceneStateHandler;
        [SerializeField] private GameView gameView;
        [SerializeField] private LeaderboardSubmitView leaderboardSubmitView;
        
        protected override void InjectInstances(IServiceLocator serviceLocator)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            serviceLocator.Add<IGameInputData>(new MobileInputData());
#else
            serviceLocator.Add<IGameInputData>(new KeyboardInputData());
#endif
            var mainCamera = Camera.main;
            serviceLocator.Add(mainCamera);
            serviceLocator.Add(new ScreenData(mainCamera));
            serviceLocator.Add(new EnemyFormationData());
            serviceLocator.Add(new PlayerData());
            
            serviceLocator.Add<ILeaderboardSubmitView>(leaderboardSubmitView);
            serviceLocator.Add(new LeaderboardSubmitController(serviceLocator));

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