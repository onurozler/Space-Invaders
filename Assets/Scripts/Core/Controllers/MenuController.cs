using System;
using System.Collections.Generic;
using Architecture.ServiceLocator;
using Core.Models.Enemy;
using Core.Models.Scene;
using Core.Models.User;
using Core.Views.Menu;
using Helpers;
using Helpers.Timing;
using Network.WebNetworkService;
using Network.WebNetworkService.Requests;
using Network.WebNetworkService.Responses;

namespace Core.Controllers
{
    public class MenuController : IDisposable
    {
        private readonly IMenuView _menuView;
        private readonly SceneData _sceneData;
        private readonly IList<EnemyAssetData> _enemyDatas;
        private readonly IWebService _webService;
        private readonly UserData _userData;
        private readonly LeaderboardController _leaderboardController;
        private readonly ITimingManager _timingManager;

        public MenuController(IServiceLocator serviceLocator)
        {
            _userData = serviceLocator.Get<UserData>();
            _webService = serviceLocator.Get<IWebService>();
            _menuView = serviceLocator.Get<IMenuView>();
            _sceneData = serviceLocator.Get<SceneData>();
            _enemyDatas = serviceLocator.Get<IList<EnemyAssetData>>();
            _leaderboardController = serviceLocator.Get<LeaderboardController>();
            _timingManager = serviceLocator.Get<ITimingManager>();
            
            _menuView.OnPlayButtonPressed += OnPlayButtonPressed;
            _menuView.OnLeaderboardButtonPressed += OnLeaderBoardButtonPressed;

            SetUserData();
            FillView();
        }

        private void SetUserData()
        {
            if (string.IsNullOrEmpty(_userData.IdToken))
            {
                _webService.SendRequest<WebRegisterResponse>(new WebRegisterRequest(), (response) =>
                {
                    _userData.Uid = response.user.id;
                    _userData.IdToken = response.idToken;
                    _userData.RefreshToken = response.refreshToken;
                    _webService.SetWebToken(response.idToken);
                });
            }
        }

        private void FillView()
        {
            var octopusScore = _enemyDatas.GetByType(EnemyType.Octopus).Score;
            var crabScore = _enemyDatas.GetByType(EnemyType.Crab).Score;
            var squidScore = _enemyDatas.GetByType(EnemyType.Squid).Score;
            
            _menuView.FillData(octopusScore,crabScore,squidScore);
        }

        private void OnPlayButtonPressed()
        {
            _sceneData.CurrentScene = SceneType.Game;
        }

        private void OnLeaderBoardButtonPressed()
        {
            _leaderboardController.SetActive(true);
        }

        public void Dispose()
        {
            _menuView.OnPlayButtonPressed -= OnPlayButtonPressed;
            _menuView.OnLeaderboardButtonPressed -= OnLeaderBoardButtonPressed;
        }
    }
}