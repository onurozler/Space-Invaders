using System;
using Architecture.ServiceLocator;
using Core.Managers;
using Core.Managers.Enemy;
using Core.Models.Bullet;
using Core.Models.Enemy;
using Helpers.Timing;
using UnityEngine;

namespace Core.Controllers.Impl
{
    public class EnemiesController : IDisposable
    {
        private readonly EnemiesManager _enemiesManager;
        private readonly EnemyFormationData _enemyFormationData;
        private readonly Coroutine _shootLogicInterval;
        private readonly ITimingManager _timingManager;
        private readonly BulletManager _bulletManager;

        public EnemiesController(IServiceLocator serviceLocator)
        {
            _enemiesManager = serviceLocator.Get<EnemiesManager>();
            _enemyFormationData = serviceLocator.Get<EnemyFormationData>();
            _timingManager = serviceLocator.Get<ITimingManager>();
            _bulletManager = serviceLocator.Get<BulletManager>();

            _shootLogicInterval = _timingManager.SetInterval(2f, -1, OnShootLogicUpdated);
        }

        private void OnShootLogicUpdated()
        {
            var shooterPosition = _enemyFormationData.GetRandomShooter();
            if (_enemiesManager.SpawnedEnemies.TryGetValue(shooterPosition, out var shooterEnemy))
            {
                _bulletManager.FireBullet(BulletType.Enemy,shooterEnemy.transform.position,Vector3.down);
            }
        }
        
        public void Dispose()
        {
        }
    }
}