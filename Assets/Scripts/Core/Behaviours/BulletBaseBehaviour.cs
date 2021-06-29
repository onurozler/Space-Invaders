using System;
using Core.Models.Bullet;
using UnityEngine;

namespace Core.Behaviours
{
    public class BulletBaseBehaviour : MonoBehaviour
    {
        [Header("Components")] 
        [SerializeField] private SpriteRenderer spriteRenderer;
        
        [Header("Bullet Sprites")]
        [SerializeField] private Sprite enemyBullet;
        [SerializeField] private Sprite playerBullet;
        
        private Vector3 _direction;
        private Action<BulletBaseBehaviour> _onDestroyed;
        
        public void Initialize(BulletType bulletType, Action<BulletBaseBehaviour> onDestroyed)
        {
            //spriteRenderer.sprite = bulletType == BulletType.Player ? playerBullet : enemyBullet;
            _onDestroyed = onDestroyed;
        }

        public void SetPositionAndDirection(Vector3 origin, Vector3 direction)
        {
            transform.position = origin;
            _direction = direction;
        }
        
        public void Move()
        {
            transform.Translate(_direction * Time.deltaTime * 5);

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
                gameObject.SetActive(false);
                _onDestroyed?.Invoke(this);
            }
        }
    }
}