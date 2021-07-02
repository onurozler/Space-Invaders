using System;
using Core.Models.Enemy;
using UnityEngine;

namespace Core.Behaviours
{
    public class EnemyBehaviour : MonoBehaviour, IKillableBehaviour
    {
        [Header("Components")]
        [SerializeField]
        private SpriteRenderer spriteRenderer;

        public event Action<EnemyBehaviour> OnEnemyKilled; 
        public Vector2Int Position { get; private set; }
        public int Score { get; private set; }
        public Sprite[] enemyAnim;
        
        private float _pace;
        
        public void Initialize(EnemyAssetData assetData, Color color, Vector2Int position)
        {
            _pace = 0.2f;
            spriteRenderer.color = color;
            spriteRenderer.sprite = assetData.Sprites[0];
            enemyAnim = assetData.Sprites;
            
            Position = position;
            Score = assetData.Score;
        }

        public void Move(Vector3 direction)
        {
            spriteRenderer.sprite = spriteRenderer.sprite == enemyAnim[0] ? enemyAnim[1] : enemyAnim[0];
            transform.Translate(direction * _pace);
        }

        public void MoveWithoutPace(Vector3 direction)
        {
            transform.Translate(direction);
        }

        public void SetPacing(float pace)
        {
            _pace += pace;
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
