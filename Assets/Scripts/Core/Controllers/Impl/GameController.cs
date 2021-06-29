using System;
using Architecture.ServiceLocator;
using Core.Models;
using Core.Models.Game.Input;
using Core.Views.Game;
using Helpers.Scene;

namespace Core.Controllers.Impl
{
    public class GameController : IDisposable
    {
        private readonly IGameView _gameView;
        private readonly ISceneStateHandler _sceneStateHandler;
        private readonly ISceneController _sceneController;
        private readonly IGameInputData _gameInputData;

        public GameController(IServiceLocator serviceLocator)
        {
            _gameView = serviceLocator.Get<IGameView>();
            _sceneController = serviceLocator.Get<ISceneController>();
            _sceneStateHandler = serviceLocator.Get<ISceneStateHandler>();
            _gameInputData = serviceLocator.Get<IGameInputData>();

            _sceneStateHandler.OnUpdated += OnUpdated;
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
            _sceneStateHandler.OnUpdated -= OnUpdated;
        }
    }
}