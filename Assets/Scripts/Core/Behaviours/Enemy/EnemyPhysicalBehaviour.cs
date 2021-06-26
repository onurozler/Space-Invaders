using UnityEngine;
using UnityEngine.Events;

namespace Core.Behaviours.Enemy
{
    public class EnemyPhysicalBehaviour : MonoBehaviour
    {
        [Header("Events")]
        public UnityEvent OnBulletCollision;

        private void OnCollisionEnter(Collision other)
        {
            OnBulletCollision?.Invoke();
        }
    }
}
