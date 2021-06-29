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

        public event Action<EnemyBaseBehaviour> OnEnemyKilled; 
        public Vector2Int Position { get; private set; }
        public int Score { get; private set; }

        public void Initialize(EnemyAssetData assetData, Color color, Vector2Int position)
        {
            spriteRenderer.color = color;
            spriteRenderer.sprite = assetData.Sprites[0];
            
            Position = position;
            Score = assetData.Score;
        }

        public void Kill()
        {
            OnEnemyKilled?.Invoke(this);
        }

        public void DeactivateAndKillAnimation()
        {
            gameObject.SetActive(false);
        }
    }
}
