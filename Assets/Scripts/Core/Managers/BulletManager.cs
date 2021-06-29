using System.Collections.Generic;
using Architecture.ServiceLocator;
using Core.Behaviours;
using Core.Managers.Pool;
using Core.Models.Bullet;
using Helpers.Scene;
using Helpers.Timing;
using UnityEngine;

namespace Core.Managers
{
    public class BulletManager : MonoBehaviour
    {
        [SerializeField]
        private MonoPoolManager bulletPool;

        private ISceneStateHandler _sceneStateHandler;
        private ICollection<BulletBaseBehaviour> _activeBullets;
        private IDictionary<BulletType, bool> _bulletRates;
        private ITimingManager _timingManager;

        public void Initialize(IServiceLocator serviceLocator)
        {
            bulletPool.Initialize();
            _bulletRates = new Dictionary<BulletType, bool>();
            _activeBullets = new List<BulletBaseBehaviour>();
            _timingManager = serviceLocator.Get<ITimingManager>();
            _sceneStateHandler = serviceLocator.Get<ISceneStateHandler>();
            _sceneStateHandler.OnUpdated += OnUpdated;
        }
        
        public void FireBullet(BulletType type ,Vector3 source, Vector3 direction)
        {
            var bullet = bulletPool.GetItem<BulletBaseBehaviour>();
            bullet.Initialize(type, OnBulletDestroyed);
            bullet.SetPositionAndDirection(source,direction);
            _activeBullets.Add(bullet);
        }

        private void OnUpdated()
        {
            foreach (var activeBullet in _activeBullets)
            {
                activeBullet.Move();
            }
        }
        
        private void OnBulletDestroyed(BulletBaseBehaviour bullet)
        {
            _activeBullets.Remove(bullet);
        }

        private void OnDestroy()
        {
            _sceneStateHandler.OnUpdated -= OnUpdated;
        }
    }
}