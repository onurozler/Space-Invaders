using System;
using System.Collections.Generic;
using Architecture.ServiceLocator;
using Core.Models;
using Core.Models.Enemy;
using Core.Views.Menu;
using Helpers;

namespace Core.Controllers.Impl
{
    public class MenuController : IDisposable
    {
        private readonly IMenuView _menuView;
        private readonly ISceneController _sceneController;
        private readonly IList<EnemyAssetData> _enemyDatas;

        public MenuController(IServiceLocator serviceLocator)
        {
            _menuView = serviceLocator.Get<IMenuView>();
            _sceneController = serviceLocator.Get<ISceneController>();
            _enemyDatas = serviceLocator.Get<IList<EnemyAssetData>>();
            
            _menuView.OnPlayButtonPressed += OnPlayButtonPressed;
            _menuView.OnLeaderboardButtonPressed += OnLeaderBoardButtonPressed;

            FillView();
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
            _sceneController.Load(Constants.Scene.GameName);
        }

        private void OnLeaderBoardButtonPressed()
        {
        }
        
        public void Dispose()
        {
            _menuView.OnPlayButtonPressed -= OnPlayButtonPressed;
            _menuView.OnLeaderboardButtonPressed -= OnLeaderBoardButtonPressed;
        }
    }
}