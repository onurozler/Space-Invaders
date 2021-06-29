using System.Collections.Generic;
using Architecture.ServiceLocator;
using Core.Behaviours.Enemy;
using Core.Models;
using Core.Models.Enemy;
using Core.Models.Player;
using Helpers;
using UnityEngine;

namespace Core.Managers
{
    public class EnemiesManager : MonoPoolManagerBase
    {
        public IDictionary<Vector2Int, EnemyBaseBehaviour> SpawnedEnemies { get; private set; }
        
        private IList<EnemyAssetData> _enemyAssetDatas;
        private EnemyFormationData _enemyFormationData;
        private PlayerData _playerData;
        
        public void Initialize(IServiceLocator serviceLocator)
        {
            SpawnedEnemies = new Dictionary<Vector2Int, EnemyBaseBehaviour>();
            
            _enemyAssetDatas = serviceLocator.Get<IList<EnemyAssetData>>();
            _enemyFormationData = serviceLocator.Get<EnemyFormationData>();
            _playerData = serviceLocator.Get<PlayerData>();
            
            InitializePool(CreateEnemies);
        }
        
        private void CreateEnemies()
        {
            var enemyGeneratedDatas = _enemyFormationData.Create(Constants.Game.Grid.x,Constants.Game.Grid.y);
            for (int y = 0; y < enemyGeneratedDatas.GetLength(1); y++)
            {
                for (int x = 0; x < enemyGeneratedDatas.GetLength(0); x++)
                {
                    var enemyData = enemyGeneratedDatas[x, y];
                    var enemyAssetData = _enemyAssetDatas.GetByType(enemyData.Type);

                    var dataPosition = new Vector2Int(x, y);
                    var enemyBase = GetItem<EnemyBaseBehaviour>();
                    enemyBase.Initialize(enemyAssetData, enemyData.Color,dataPosition);
                    enemyBase.OnEnemyKilled += OnEnemyKilled;
                    
                    var position = new Vector2(x, y);
                    enemyBase.transform.localPosition = position;

                    SpawnedEnemies[dataPosition] = enemyBase;
                }
            }
        }

        private void OnEnemyKilled(EnemyBaseBehaviour enemy)
        {
            var connectedEnemies = _enemyFormationData.GetConnectedEnemies(enemy.Position);
            foreach (var connectedEnemyPosition in connectedEnemies)
            {
                _enemyFormationData.Remove(connectedEnemyPosition);
                
                var targetEnemy = SpawnedEnemies[connectedEnemyPosition];
                targetEnemy.OnEnemyKilled -= OnEnemyKilled;
                targetEnemy.DeactivateAndKillAnimation();

                _playerData.Score += enemy.Score;
            }
        }
    }
}