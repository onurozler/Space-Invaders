using System;
using Core.Models.Game;
using Core.Models.Ship;
using Helpers.Scene;
using UnityEngine;

namespace Core.Behaviours
{
    public class ShipBehaviour : MonoBehaviour, IKillableBehaviour, IMoveableBehaviour
    {
        [SerializeField]
        private SpriteRenderer spriteRenderer;

        public event Action<ShipBehaviour,bool> OnDestroyed;

        public ShipType ShipType { get; private set; }
        public int Score { get; private set; }
        
        private Vector3 _direction;
        private ShipAssetData _shipAssetData;
        private ISceneStateHandler _sceneStateHandler;
        private float _screenOutXPoint;

        public void Initialize(ShipAssetData shipAssetData, ISceneStateHandler sceneStateHandler, ScreenData screenData)
        {
            spriteRenderer.sprite = shipAssetData.Sprite;
            spriteRenderer.color = shipAssetData.Color;

            ShipType = shipAssetData.ShipType;
            Score = shipAssetData.Score;

            _shipAssetData = shipAssetData;
            _sceneStateHandler = sceneStateHandler;
            _screenOutXPoint = screenData.GetWidthBoundary().Max;

            _sceneStateHandler.OnUpdated += CheckIfOutOfScreen;
        }

        private void CheckIfOutOfScreen()
        {
            if (transform.position.x > _screenOutXPoint)
            {
                gameObject.SetActive(false);
                OnDestroyed?.Invoke(this,true);
                _sceneStateHandler.OnUpdated -= CheckIfOutOfScreen;
            }
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
            OnDestroyed?.Invoke(this,false);
            _sceneStateHandler.OnUpdated -= CheckIfOutOfScreen;
        }
    }
}