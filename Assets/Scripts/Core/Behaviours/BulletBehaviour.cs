using System;
using Core.Models;
using Core.Models.Bullet;
using Core.Models.Game;
using Helpers.Scene;
using UnityEngine;

namespace Core.Behaviours
{
    public class BulletBehaviour : MonoBehaviour, IKillableBehaviour
    {
        [Header("Components")] 
        [SerializeField] private SpriteRenderer spriteRenderer;
        
        [Header("Bullet Sprites")]
        [SerializeField] private Sprite enemyBullet;
        [SerializeField] private Sprite playerBullet;
        
        private Vector3 _direction;
        private ISceneStateHandler _sceneStateHandler;
        private ScreenBoundary _heightBoundary;
        public event Action<BulletBehaviour> OnDestroyed;
        
        public void Initialize(BulletType bulletType, ISceneStateHandler sceneStateHandler, ScreenData screenData)
        {
            _heightBoundary = screenData.GetHeightBoundary();
            _sceneStateHandler = sceneStateHandler;
            var isPlayerBullet = bulletType == BulletType.Player;
            gameObject.layer = isPlayerBullet ? Constants.Game.PlayerLayer : Constants.Game.EnemyLayer;
            //spriteRenderer.sprite = bulletType == BulletType.Player ? playerBullet : enemyBullet;
            _sceneStateHandler.OnUpdated += OnUpdated;
        }

        private void OnUpdated()
        {
            if (transform.position.y < _heightBoundary.Min)
            {
                Kill();
            }
            else if (transform.position.y > _heightBoundary.Max)
            {
                Kill();
            }
        }

        public void SetPositionAndDirection(Vector3 origin, Vector3 direction)
        {
            transform.position = origin;
            _direction = direction;
        }
        
        public void Move()
        {
            transform.Translate(_direction * Time.deltaTime * Constants.Game.BulletSpeed);
        }
        
        private void OnCollisionEnter2D(Collision2D other)
        {
            var killableBehaviour = other.gameObject.GetComponent<IKillableBehaviour>();
            if (killableBehaviour != null)
            {
                killableBehaviour.Kill();
                Kill();
            }
        }

        public void Kill()
        {
            gameObject.SetActive(false);
            OnDestroyed?.Invoke(this);
            _sceneStateHandler.OnUpdated -= OnUpdated;
        }
    }
}