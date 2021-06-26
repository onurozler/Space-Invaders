using UnityEngine;

namespace Core.Models.Enemy
{
    [CreateAssetMenu(fileName = "EnemyData", menuName = "Space Invaders/Create Enemy Data", order = 1)]
    public class EnemyData : ScriptableObject, IEnemyData
    {
        [SerializeField]
        private EnemyType type;

        [SerializeField] 
        private int scorePoint;

        private Color _enemyColor;

        public EnemyType Type => type;
        public int Score => scorePoint;
    }

    public enum EnemyType
    {
        Squid,
        Crab,
        Octopus
    }
}