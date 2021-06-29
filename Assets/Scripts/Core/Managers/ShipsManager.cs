using System;
using System.Collections.Generic;
using Architecture.ServiceLocator;
using Core.Behaviours;
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

        private IList<IMoveableBehaviour> _activeMoveables;
        
        public void Initialize(IServiceLocator serviceLocator)
        {
            _activeMoveables = new List<IMoveableBehaviour>();
            _shipAssetDatas = serviceLocator.Get<IList<ShipAssetData>>();
            _timingManager = serviceLocator.Get<ITimingManager>();
            _sceneStateHandler = serviceLocator.Get<ISceneStateHandler>();
            _playerData = serviceLocator.Get<PlayerData>();
            _timingManager.SetInterval(shipInterval, -1, CreateShip);
            InitializePool();

            _sceneStateHandler.OnUpdated += OnUpdated;
        }

        private void OnUpdated()
        {
            foreach (var moveable in _activeMoveables)
            {
                moveable.Move();
            }
        }

        private void CreateShip()
        {
            var ship = GetItem<ShipBehaviour>();
            ship.Initialize(_shipAssetDatas.GetRandom());
            ship.SetPositionAndDirection(new Vector3(-6,3,0),Vector3.right);
            ship.OnDestroyed += OnShipDestroyed;
            
            _activeMoveables.Add(ship);
        }

        private void OnShipDestroyed(ShipBehaviour shipBehaviour)
        {
            _activeMoveables.Remove(shipBehaviour);
            
            shipBehaviour.OnDestroyed -= OnShipDestroyed;
            if (shipBehaviour.ShipType == ShipType.Mystery)
            {
                
            }
            else
            {
                _playerData.Score += shipBehaviour.Score;
            }
            // Increase player socre
        }
    }
}