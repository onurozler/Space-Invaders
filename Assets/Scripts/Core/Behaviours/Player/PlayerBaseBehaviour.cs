using Architecture.ServiceLocator;
using Core.Managers;
using Core.Models.Bullet;
using Core.Models.Game;
using Core.Models.Game.Input;
using Core.Models.Player;
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
        private PlayerData _playerData;
        private ScreenBoundary _screenWidthBoundary;
        private bool _canShoot;
        
        public void Initialize(IServiceLocator serviceLocator)
        {
            _gameInputData = serviceLocator.Get<IGameInputData>();
            _sceneStateHandler = serviceLocator.Get<ISceneStateHandler>();
            _bulletManager = serviceLocator.Get<BulletManager>();
            _timingManager = serviceLocator.Get<ITimingManager>();
            _playerData = serviceLocator.Get<PlayerData>();
            _screenWidthBoundary = serviceLocator.Get<ScreenData>().GetWidthBoundary();
            _canShoot = true;
            _sceneStateHandler.OnUpdated += OnUpdated;
        }

        private void OnUpdated()
        {
            _playerData.Lives = Lives;

            switch (_gameInputData.FirstInput)
            {
                case InputState.Left:
                    if (transform.position.x < _screenWidthBoundary.Min + 0.5f)
                        return;

                    transform.Translate(Vector3.left * Time.deltaTime * Speed);
                    break;
                case InputState.Right:
                    if (transform.position.x > _screenWidthBoundary.Max - 0.5f)
                        return;
                    
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
            Lives--;
            if (Lives == 0)
            {
                _playerData.PlayerState = PlayerState.GameOver;
                gameObject.SetActive(false);
            }
        }

        private void OnDestroy()
        {
            _sceneStateHandler.OnUpdated -= OnUpdated;
        }
    }
}