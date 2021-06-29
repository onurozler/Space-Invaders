using Architecture.ServiceLocator;
using Core.Managers;
using Core.Models.Bullet;
using Core.Models.Game.Input;
using Helpers.Scene;
using Helpers.Timing;
using UnityEngine;

namespace Core.Behaviours.Player
{
    public class PlayerBaseBehaviour : MonoBehaviour, IKillableBehaviour
    {
        [Header("Runtime Data")]
        [Range(2,10)]
        public float Speed;
        [Range(0,5)]
        public float BulletRate;
        public int Lives;

        private ISceneStateHandler _sceneStateHandler;
        private IGameInputData _gameInputData;
        private BulletManager _bulletManager;
        private ITimingManager _timingManager;
        private bool _canShoot;
        
        public void Initialize(IServiceLocator serviceLocator)
        {
            _gameInputData = serviceLocator.Get<IGameInputData>();
            _sceneStateHandler = serviceLocator.Get<ISceneStateHandler>();
            _bulletManager = serviceLocator.Get<BulletManager>();
            _timingManager = serviceLocator.Get<ITimingManager>();
            _canShoot = true;
            _sceneStateHandler.OnUpdated += OnUpdated;
        }

        private void OnUpdated()
        {
            switch (_gameInputData.FirstInput)
            {
                case InputState.Left:
                    transform.Translate(Vector3.left * Time.deltaTime * Speed);
                    break;
                case InputState.Right:
                    transform.Translate(Vector3.right * Time.deltaTime * Speed);
                    break;
            }

            if (_gameInputData.SecondInput == InputState.Shoot && _canShoot)
            {
                _canShoot = false;
                _timingManager.SetInterval(BulletRate,()=>_canShoot = true);
                _bulletManager.FireBullet(BulletType.Player,transform.position,Vector3.up);
            }
        }

        public void Kill()
        {
            
        }

        private void OnDestroy()
        {
            _sceneStateHandler.OnUpdated -= OnUpdated;
        }
    }
}