using System;
using Core.Models;
using Core.Models.Bullet;
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
        public event Action<BulletBehaviour> OnDestroyed;
        
        public void Initialize(BulletType bulletType)
        {
            var isPlayerBullet = bulletType == BulletType.Player;
            gameObject.layer = isPlayerBullet ? Constants.Game.PlayerLayer : Constants.Game.EnemyLayer;
            //spriteRenderer.sprite = bulletType == BulletType.Player ? playerBullet : enemyBullet;
        }

        public void SetPositionAndDirection(Vector3 origin, Vector3 direction)
        {
            transform.position = origin;
            _direction = direction;
        }
        
        public void Move()
        {
            transform.Translate(_direction * Time.deltaTime * Constants.Game.BulletSpeed);

            //  TODO : If out of screen
            // if (false)
            // {
            //     _onDestroyed?.Invoke(this);
            // }
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
        }
    }
}