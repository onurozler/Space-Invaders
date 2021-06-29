using UnityEngine;

namespace Core.Behaviours
{
    public interface IMoveableBehaviour
    {
        void SetPositionAndDirection(Vector3 origin, Vector3 direction);
        void Move();
    }
}