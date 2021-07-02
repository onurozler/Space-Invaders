using System.Collections.Generic;
using Architecture.ServiceLocator;
using Core.Behaviours;
using Core.Models.Game;
using Core.Models.Player;
using Core.Models.Ship;
using Helpers;
using Helpers.Scene;
using Helpers.Timing;
using UnityEngine;

namespace Core.Managers
{
    public class ShipsManager : MonoPoolManagerBase
    {
        [Header("Ship Options")]
        [SerializeField] 
        private int shipInterval;
        
        private IList<ShipAssetData> _shipAssetDatas;
        private ITimingManager _timingManager;
        private ISceneStateHandler _sceneStateHandler;
        private PlayerData _playerData;
        private ScreenData _screenData;

        private IList<ShipBehaviour> _activeShips;
        
        public void Initialize(IServiceLocator serviceLocator)
        {
            _activeShips = new List<ShipBehaviour>();
            _shipAssetDatas = serviceLocator.Get<IList<ShipAssetData>>();
            _timingManager = serviceLocator.Get<ITimingManager>();
            _sceneStateHandler = serviceLocator.Get<ISceneStateHandler>();
            _playerData = serviceLocator.Get<PlayerData>();
            _screenData = serviceLocator.Get<ScreenData>();
            _timingManager.SetInterval(shipInterval, -1, CreateShip);
            
            InitializePool();

            _sceneStateHandler.OnUpdated += OnUpdated;
        }

        private void OnUpdated()
        {
            foreach (var ship in _activeShips)
            {
                ship.Move();
            }
        }

        private void CreateShip()
        {
            var ship = GetItem<ShipBehaviour>();
            ship.Initialize(_shipAssetDatas.GetRandom(),_sceneStateHandler,_screenData);
            ship.SetPositionAndDirection(new Vector3(_screenData.GetWidthBoundary().Min - 2f,6,0),Vector3.right);
            ship.OnDestroyed += OnShipDestroyed;
            
            _activeShips.Add(ship);
        }

        private void OnShipDestroyed(ShipBehaviour shipBehaviour, bool destroyedItself)
        {
            _activeShips.Remove(shipBehaviour);
            if(destroyedItself)
                return;
            
            var middlePointX = _screenData.GetMiddlePoint().x;
            var startPointX = _screenData.GetWidthBoundary().Min;
            var screenEndPointX = _screenData.GetWidthBoundary().Max;

            shipBehaviour.OnDestroyed -= OnShipDestroyed;
            if (shipBehaviour.ShipType == ShipType.Mystery)
            {
                float percentage;
                if (shipBehaviour.transform.position.x <= middlePointX)
                {
                    percentage = Mathf.InverseLerp(startPointX, middlePointX, shipBehaviour.transform.position.x);
                }
                else
                {
                    percentage = Mathf.InverseLerp(screenEndPointX, middlePointX, shipBehaviour.transform.position.x);
                }

                _playerData.Score += (int) (shipBehaviour.Score * percentage);
            }
            else
            {
                _playerData.Score += shipBehaviour.Score;
            }
        }
    }
}