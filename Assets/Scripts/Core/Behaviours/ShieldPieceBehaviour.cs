using UnityEngine;

namespace Core.Behaviours
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class ShieldPieceBehaviour : MonoBehaviour, IKillableBehaviour
    {
        [SerializeField]
        private SpriteRenderer spriteRenderer;
        
        [SerializeField] 
        private Sprite splatSprite;
        
        private int _tolerance;
        private int _current;
        
        public void Initialize(int tolerance)
        {
            _current = 0;
            _tolerance = tolerance;
        }

        public void Kill()
        {
            _current++;
            if (_current == Mathf.FloorToInt(_tolerance * 0.5f))
            {
                spriteRenderer.sprite = splatSprite;
            }
            
            if (_current == _tolerance)
            {
                gameObject.SetActive(false);
            }        
        }
    }
}