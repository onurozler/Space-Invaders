using System.Collections.Generic;
using Architecture.ServiceLocator;
using Core.Behaviours.Enemy;
using Core.Managers.Pool;
using Core.Models;
using Core.Models.Enemy;
using Helpers;
using UnityEngine;

namespace Core.Managers.Enemy
{
    public class EnemiesManager : MonoBehaviour
    {
        public IDictionary<Vector2Int, EnemyBaseBehaviour> SpawnedEnemies { get; private set; }
        
        [SerializeField] private MonoPoolManager enemyPoolManager;

        private IList<EnemyAssetData> _enemyAssetDatas;
        private EnemyFormationData _enemyFormationData;
        
        public void Initialize(IServiceLocator serviceLocator)
        {
            SpawnedEnemies = new Dictionary<Vector2Int, EnemyBaseBehaviour>();
            
            _enemyAssetDatas = serviceLocator.Get<IList<EnemyAssetData>>();
            _enemyFormationData = serviceLocator.Get<EnemyFormationData>();
            enemyPoolManager.Initialize(CreateEnemies);
        }
        
        private void CreateEnemies()
        {
            var enemyGeneratedDatas = _enemyFormationData.Create();
            for (int y = 0; y < enemyGeneratedDatas.GetLength(1); y++)
            {
                for (int x = 0; x < enemyGeneratedDatas.GetLength(0); x++)
                {
                    var enemyData = enemyGeneratedDatas[x, y];
                    var enemyAssetData = _enemyAssetDatas.GetByType(enemyData.Type);

                    var dataPosition = new Vector2Int(x, y);
                    var enemyBase = enemyPoolManager.GetItem<EnemyBaseBehaviour>();
                    enemyBase.Initialize(enemyAssetData, enemyData.Color,dataPosition);
                    enemyBase.OnEnemyKilled += OnEnemyKilled;
                    
                    var position = new Vector2(x, y);
                    enemyBase.transform.localPosition = position;

                    SpawnedEnemies[dataPosition] = enemyBase;
                }
            }
        }

        private void OnEnemyKilled(Vector2Int position)
        {
            var connectedEnemies = _enemyFormationData.GetConnectedEnemies(position);
            foreach (var connectedEnemyPosition in connectedEnemies)
            {
                _enemyFormationData.Remove(connectedEnemyPosition);
                
                var targetEnemy = SpawnedEnemies[connectedEnemyPosition];
                targetEnemy.OnEnemyKilled -= OnEnemyKilled;
                targetEnemy.DeactivateAndKillAnimation();
            }
        }
    }
}