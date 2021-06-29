using UnityEngine;

namespace Core.Models.Enemy
{
    [CreateAssetMenu(fileName = "EnemyAssetData", menuName = "Space Invaders/Create Enemy Asset Data", order = 1)]
    public class EnemyAssetData : ScriptableObject
    {
        public EnemyType Type;
        public Sprite[] Sprites;
        public int Score;
    }

    public enum EnemyType
    {
        Squid = 0,
        Crab,
        Octopus
    }
}