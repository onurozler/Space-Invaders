using Core.Models.Enemy;
using UnityEngine;

namespace Core.Behaviours.Enemy
{
    public class EnemyBaseBehaviour : MonoBehaviour
    {
        [Header("Data")]
        [SerializeField] 
        private EnemyData enemyData;

        [Header("Components")]
        [SerializeField]
        private SpriteRenderer spriteRenderer;
        
        [Header("Sub-Behaviours")]
        [SerializeField]
        private EnemyPhysicalBehaviour physicalBehaviour;

    }
}
