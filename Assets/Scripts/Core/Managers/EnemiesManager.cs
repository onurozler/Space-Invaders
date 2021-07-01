using System.Collections.Generic;
using System.Linq;
using Architecture.ServiceLocator;
using Core.Behaviours.Enemy;
using Core.Models;
using Core.Models.Enemy;
using Core.Models.Game;
using Core.Models.Player;
using Helpers;
using Helpers.Timing;
using UnityEngine;

namespace Core.Managers
{
    public class EnemiesManager : MonoPoolManagerBase
    {
        [SerializeField]
        [Range(0,0.05f)]
        private float pacing;
        
        public IDictionary<Vector2Int, EnemyBaseBehaviour> SpawnedEnemies { get; private set; }
        
        private IList<EnemyAssetData> _enemyAssetDatas;
        private EnemyFormationData _enemyFormationData;
        private ITimingManager _timingManager;
        private PlayerData _playerData;
        private ScreenBoundary _widthBoundary;
        private bool _isRightDirection;
        private bool _isDirectionChanged;
        
        public void Initialize(IServiceLocator serviceLocator)
        {
            SpawnedEnemies = new Dictionary<Vector2Int, EnemyBaseBehaviour>();

            _isRightDirection = true;
            _enemyAssetDatas = serviceLocator.Get<IList<EnemyAssetData>>();
            _enemyFormationData = serviceLocator.Get<EnemyFormationData>();
            _widthBoundary = serviceLocator.Get<ScreenData>().GetWidthBoundary();
            _timingManager = serviceLocator.Get<ITimingManager>();
            _playerData = serviceLocator.Get<PlayerData>();
            
            InitializePool(CreateEnemies);

            _timingManager.SetInterval(1f, -1, OnEnemiesMove);
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
                    
                    var position = new Vector2(x + (x * 0.5f), y);
                    enemyBase.transform.localPosition = position;

                    SpawnedEnemies[dataPosition] = enemyBase;
                }
            }
        }
        
        private void OnEnemiesMove()
        {
            for (int i = 0; i < SpawnedEnemies.Values.Count; i++)
            {
                var enemy = SpawnedEnemies.Values.ElementAt(i);
                if (_isRightDirection && enemy.transform.position.x > (_widthBoundary.Max - 0.5f))
                {
                    _isRightDirection = false;
                    MoveDownWithPacing();
                }
                else if (!_isRightDirection && enemy.transform.position.x < (_widthBoundary.Min + 0.5f))
                {
                    _isRightDirection = true;
                    MoveDownWithPacing();
                }
                
                _timingManager.SetInterval(0.01f * i, () =>
                {
                    enemy.Move(_isRightDirection ? Vector3.right : Vector3.left);
                });
            }
        }

        private void MoveDownWithPacing()
        {
            for (int i = 0; i < SpawnedEnemies.Values.Count; i++)
            {
                var enemy = SpawnedEnemies.Values.ElementAt(i);
                enemy.SetPacing(pacing);
                _timingManager.SetInterval(0.01f * i, () =>
                {
                    enemy.MoveWithoutPace(Vector3.down * 0.25f);
                });
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