using System;
using Architecture.ServiceLocator;
using Core.Models.Player;
using Core.Models.Scene;
using Core.Views.Leaderboard;
using Helpers.Logger;
using Network.WebNetworkService;
using Network.WebNetworkService.Requests;
using Network.WebNetworkService.Responses;

namespace Core.Controllers
{
    public class LeaderboardSubmitController : IDisposable
    {
        private readonly PlayerData _playerData;
        private readonly ILeaderboardSubmitView _leaderboardSubmitView;
        private readonly SceneData _sceneData;
        private readonly IWebService _webService;
        
        public LeaderboardSubmitController(IServiceLocator serviceLocator)
        {
            _webService = serviceLocator.Get<IWebService>();
            _playerData = serviceLocator.Get<PlayerData>();
            _sceneData = serviceLocator.Get<SceneData>();
            _leaderboardSubmitView = serviceLocator.Get<ILeaderboardSubmitView>();
            _leaderboardSubmitView.OnSubmitButtonPressed += OnSubmitButtonPressed;
        }

        public void Show()
        {
            _leaderboardSubmitView.Activate();
        }

        private void OnSubmitButtonPressed(string playerName)
        {
            var leaderboardPostRequest = new WebLeaderboardPostRequest();
            leaderboardPostRequest.Parameters.Add("tournamentId","dk");
            leaderboardPostRequest.Parameters.Add("name",playerName);
            leaderboardPostRequest.Parameters.Add("score",_playerData.Score.ToString());
            
            _webService.SendRequest<WebLeaderboardSubmitResponse>(leaderboardPostRequest, (response) =>
            {
                if (response.code == 1)
                {
                    _sceneData.CurrentScene = SceneType.Menu;
                }
                else
                {
                    Logger.Log("Leaderboard Submit Error!");
                }
            });
        }

        public void Dispose()
        {
            _leaderboardSubmitView.OnSubmitButtonPressed -= OnSubmitButtonPressed;
        }
    }
}