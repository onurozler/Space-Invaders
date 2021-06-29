using System.Collections.Generic;
using Architecture.ServiceLocator;
using Core.Behaviours;
using Core.Models.Bullet;
using Helpers.Scene;
using UnityEngine;

namespace Core.Managers
{
    public class BulletManager : MonoPoolManagerBase
    {
        private ISceneStateHandler _sceneStateHandler;
        private ICollection<BulletBehaviour> _activeBullets;

        public void Initialize(IServiceLocator serviceLocator)
        {
            InitializePool();
            _activeBullets = new List<BulletBehaviour>();
            _sceneStateHandler = serviceLocator.Get<ISceneStateHandler>();
            _sceneStateHandler.OnUpdated += OnUpdated;
        }
        
        public void FireBullet(BulletType type ,Vector3 source, Vector3 direction)
        {
            var bullet = GetItem<BulletBehaviour>();
            bullet.Initialize(type);
            bullet.SetPositionAndDirection(source,direction);
            bullet.OnDestroyed += OnBulletDestroyed;
            _activeBullets.Add(bullet);
        }

        private void OnUpdated()
        {
            foreach (var activeBullet in _activeBullets)
            {
                activeBullet.Move();
            }
        }
        
        private void OnBulletDestroyed(BulletBehaviour bullet)
        {
            bullet.OnDestroyed -= OnBulletDestroyed;
            _activeBullets.Remove(bullet);
        }

        private void OnDestroy()
        {
            _sceneStateHandler.OnUpdated -= OnUpdated;
        }
    }
}