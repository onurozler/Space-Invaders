using System;
using Architecture.ServiceLocator;
using Core.Managers;
using Core.Models.Bullet;
using Core.Models.Enemy;
using Helpers.Scene;
using Helpers.Timing;
using UnityEngine;

namespace Core.Controllers
{
    public class EnemiesController : IDisposable
    {
        private readonly EnemiesManager _enemiesManager;
        private readonly EnemyFormationData _enemyFormationData;
        private readonly Coroutine _shootLogicInterval;
        private readonly ITimingManager _timingManager;
        private readonly BulletManager _bulletManager;
        private readonly ISceneStateHandler _sceneStateHandler;

        public EnemiesController(IServiceLocator serviceLocator)
        {
            _enemiesManager = serviceLocator.Get<EnemiesManager>();
            _enemyFormationData = serviceLocator.Get<EnemyFormationData>();
            _timingManager = serviceLocator.Get<ITimingManager>();
            _bulletManager = serviceLocator.Get<BulletManager>();
            _sceneStateHandler = serviceLocator.Get<ISceneStateHandler>();
            
            _shootLogicInterval = _timingManager.SetInterval(2f, -1, OnShootLogicUpdated);

            _sceneStateHandler.OnUpdated += OnEnemiesMovement;
        }
        
        private void OnShootLogicUpdated()
        {
            var shooterPosition = _enemyFormationData.GetRandomShooter();
            if (_enemiesManager.SpawnedEnemies.TryGetValue(shooterPosition, out var shooterEnemy))
            {
                _bulletManager.FireBullet(BulletType.Enemy,shooterEnemy.transform.position,Vector3.down);
            }
        }
        
        
        private void OnEnemiesMovement()
        {
            
        }
        
        public void Dispose()
        {
            _sceneStateHandler.OnUpdated -= OnEnemiesMovement;
        }
    }
}