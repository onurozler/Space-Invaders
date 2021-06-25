using System;
using Architecture.ServiceLocator;
using Core.Models;
using Core.Views.Menu;

namespace Core.Controllers
{
    public class MenuController : IDisposable
    {
        private readonly IMenuView _menuView;
        private readonly ISceneController _sceneController;
        
        public MenuController(IServiceLocator serviceLocator)
        {
            _menuView = serviceLocator.Get<IMenuView>();
            _sceneController = serviceLocator.Get<ISceneController>();
            
            _menuView.OnPlayButtonPressed += OnPlayButtonPressed;
            _menuView.OnLeaderboardButtonPressed += OnLeaderBoardButtonPressed;
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