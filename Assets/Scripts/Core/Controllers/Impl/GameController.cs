using System;
using Architecture.ServiceLocator;
using Core.Models;
using Core.Models.Enemy;
using Core.Models.Game;
using Core.Models.Game.Input;
using Core.Models.Player;
using Core.Views.Game;
using Helpers.Scene;
using Helpers.Timing;
using UnityEngine;

namespace Core.Controllers.Impl
{
    public class GameController : IDisposable
    {
        private readonly IGameView _gameView;
        private readonly ISceneStateHandler _sceneStateHandler;
        private readonly ISceneController _sceneController;
        private readonly IGameInputData _gameInputData;
        private readonly ITimingManager _timingManager;
        private readonly GameAssetData _gameAssetData;
        private readonly EnemyFormationData _enemyFormationData;
        private readonly PlayerData _playerData;
        private readonly Coroutine _interval;
        private float _totalTime;

        public GameController(IServiceLocator serviceLocator)
        {
            _gameView = serviceLocator.Get<IGameView>();
            _sceneController = serviceLocator.Get<ISceneController>();
            _sceneStateHandler = serviceLocator.Get<ISceneStateHandler>();
            _gameInputData = serviceLocator.Get<IGameInputData>();
            _enemyFormationData = serviceLocator.Get<EnemyFormationData>();
            _playerData = serviceLocator.Get<PlayerData>();
            _timingManager = serviceLocator.Get<ITimingManager>();
            _gameAssetData = serviceLocator.Get<GameAssetData>();

            _playerData.OnLivesChanged += OnLivesChanged;
            _playerData.OnScoreChanged += OnScoreChanged;
            _playerData.OnGameOver += OnGameOver;
            _enemyFormationData.OnAllEnemiesDestroyed += OnGameFinished;
            _sceneStateHandler.OnUpdatedIgnoringPause += OnUpdated;
            
            _gameView.FillData(_gameAssetData.ExtraBonusTotalTime);
            _totalTime = _gameAssetData.ExtraBonusTotalTime;
            _interval = _timingManager.SetInterval(1f,_gameAssetData.ExtraBonusTotalTime, 
                OnUpdateExtraBonus,OnShowExtraBonus);
        }
        
        private void OnUpdateExtraBonus()
        {
            _totalTime -= 1f;
            _gameView.UpdateExtraBonus(_totalTime);
        }
        
        private void OnShowExtraBonus()
        {
            _gameView.ShowExtraBonus(_playerData.ExtraScore);
        }
        
        private void OnLivesChanged(int lives)
        {
            _gameView.SetLives(lives);
        }

        private void OnScoreChanged(int score)
        {
            var ratio = _totalTime / _gameAssetData.ExtraBonusTotalTime;
            _playerData.ExtraScore += (int)(_gameAssetData.ExtraBonusMultiplier * ratio);
            _gameView.SetScore(score);
        }

        private void OnGameOver()
        {
            _sceneController.Load(Constants.Scene.MenuName);
        }

        private void OnGameFinished()
        {
            _playerData.Score += _playerData.ExtraScore;
            _sceneController.Load(Constants.Scene.MenuName);
        }

        private void OnUpdated()
        {
            switch (_gameInputData.FirstInput)
            {
                case InputState.Pause:
                    _sceneStateHandler.PauseOrResume();
                    break;
                case InputState.Quit:
                    _sceneController.Load(Constants.Scene.MenuName);
                    break;
            }
        }

        public void Dispose()
        {
            _timingManager.Stop(_interval);
            _playerData.OnLivesChanged -= OnLivesChanged;
            _playerData.OnScoreChanged -= OnScoreChanged;
            _playerData.OnGameOver -= OnGameOver;
            _enemyFormationData.OnAllEnemiesDestroyed -= OnGameFinished;
            _sceneStateHandler.OnUpdatedIgnoringPause -= OnUpdated;
        }
    }
}