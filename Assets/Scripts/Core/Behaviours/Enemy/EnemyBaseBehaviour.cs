using System;
using Core.Models.Enemy;
using UnityEngine;

namespace Core.Behaviours.Enemy
{
    public class EnemyBaseBehaviour : MonoBehaviour, IKillableBehaviour
    {
        [Header("Components")]
        [SerializeField]
        private SpriteRenderer spriteRenderer;

        public event Action<Vector2Int> OnEnemyKilled; 
        private Vector2Int _position;

        public void Initialize(EnemyAssetData assetData, Color color, Vector2Int position)
        {
            spriteRenderer.color = color;
            spriteRenderer.sprite = assetData.Sprites[0];
            _position = position;
        }

        public void Kill()
        {
            OnEnemyKilled?.Invoke(_position);
        }

        public void DeactivateAndKillAnimation()
        {
            gameObject.SetActive(false);
        }
    }
}
