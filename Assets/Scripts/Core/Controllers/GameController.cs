using System;
using System.Linq;
using Architecture.ServiceLocator;
using Core.Models.Enemy;
using Core.Models.Game;
using Core.Models.Game.Input;
using Core.Models.Player;
using Core.Models.Scene;
using Core.Views.Game;
using Helpers.Scene;
using Helpers.Timing;
using Network.WebNetworkService;
using Network.WebNetworkService.Requests;
using Network.WebNetworkService.Responses;
using UnityEngine;

namespace Core.Controllers
{
    public class GameController : IDisposable
    {
        private readonly IGameView _gameView;
        private readonly ISceneStateHandler _sceneStateHandler;
        private readonly IGameInputData _gameInputData;
        private readonly ITimingManager _timingManager;
        private readonly GameAssetData _gameAssetData;
        private readonly EnemyFormationData _enemyFormationData;
        private readonly PlayerData _playerData;
        private readonly SceneData _sceneData;
        private readonly Coroutine _interval;
        private readonly IWebService _webService;
        private readonly LeaderboardSubmitController _leaderboardSubmitController;
        private float _totalTime;

        public GameController(IServiceLocator serviceLocator)
        {
            _gameView = serviceLocator.Get<IGameView>();
            _sceneStateHandler = serviceLocator.Get<ISceneStateHandler>();
            _gameInputData = serviceLocator.Get<IGameInputData>();
            _enemyFormationData = serviceLocator.Get<EnemyFormationData>();
            _playerData = serviceLocator.Get<PlayerData>();
            _timingManager = serviceLocator.Get<ITimingManager>();
            _gameAssetData = serviceLocator.Get<GameAssetData>();
            _sceneData = serviceLocator.Get<SceneData>();
            _webService = serviceLocator.Get<IWebService>();
            _leaderboardSubmitController = serviceLocator.Get<LeaderboardSubmitController>();

            _playerData.OnLivesChanged += OnLivesChanged;
            _playerData.OnScoreChanged += OnScoreChanged;
            _playerData.OnGameOver += OnGameOver;
            _enemyFormationData.OnAllEnemiesDestroyed += OnGameFinished;
            _sceneStateHandler.OnUpdated += OnUpdated;
            
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
            _sceneData.CurrentScene = SceneType.Menu;
        }

        private void OnGameFinished()
        {
            _playerData.Score += _playerData.ExtraScore;
            var leaderboardRequest = new WebLeaderboardRequest();
            leaderboardRequest.Parameters.Add("country","tr");
            
            _webService.SendRequest<WebLeaderboardResponse>(leaderboardRequest, (response) =>
            {
                var lowestScoredPlayer = response.@group.players.OrderBy(x => x.score).ToList()[0];
                if (_playerData.Score > lowestScoredPlayer.score)
                {
                    _leaderboardSubmitController.Show();
                }
                else
                {
                    _sceneData.CurrentScene = SceneType.Menu;
                }
            });
        }

        private void OnUpdated()
        {
            switch (_gameInputData.FirstInput)
            {
                case InputState.Pause:
                    _sceneStateHandler.PauseOrResume();
                    break;
                case InputState.Quit:
                    _sceneData.CurrentScene = SceneType.Menu;
                    break;
            }
        }

        public void Dispose()
        {
            _playerData.OnLivesChanged -= OnLivesChanged;
            _playerData.OnScoreChanged -= OnScoreChanged;
            _playerData.OnGameOver -= OnGameOver;
            _enemyFormationData.OnAllEnemiesDestroyed -= OnGameFinished;
            _sceneStateHandler.OnUpdated -= OnUpdated;
        }
    }
}