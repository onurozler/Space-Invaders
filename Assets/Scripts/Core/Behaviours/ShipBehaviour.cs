using System;
using Core.Models.Ship;
using UnityEngine;

namespace Core.Behaviours
{
    public class ShipBehaviour : MonoBehaviour, IKillableBehaviour, IMoveableBehaviour
    {
        [SerializeField]
        private SpriteRenderer spriteRenderer;

        public event Action<ShipBehaviour> OnDestroyed;

        public ShipType ShipType { get; private set; }
        public int Score { get; private set; }
        
        private Vector3 _direction;
        private ShipAssetData _shipAssetData;

        public void Initialize(ShipAssetData shipAssetData)
        {
            spriteRenderer.sprite = shipAssetData.Sprite;
            spriteRenderer.color = shipAssetData.Color;

            ShipType = shipAssetData.ShipType;
            Score = shipAssetData.Score;

            _shipAssetData = shipAssetData;
        }

        public void SetPositionAndDirection(Vector3 origin, Vector3 direction)
        {
            transform.position = origin;
            _direction = direction;
        }

        public void Move()
        {
            transform.Translate(_direction * Time.deltaTime * _shipAssetData.Speed);
        }
        
        public void Kill()
        {
            gameObject.SetActive(false);
            OnDestroyed?.Invoke(this);
        }
    }
}