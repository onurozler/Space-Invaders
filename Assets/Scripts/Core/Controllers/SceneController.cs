using System;
using Architecture.ServiceLocator;
using Core.Models.Scene;
using Helpers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core.Controllers
{
    public class SceneController : IDisposable
    {
        private readonly SceneData _sceneData;
        private readonly ResolutionHelper _resolutionHelper;
        private AsyncOperation _currentLoadOperation;
            
        public SceneController(IServiceLocator serviceLocator)
        {
            _sceneData = serviceLocator.Get<SceneData>();
            _resolutionHelper = serviceLocator.Get<ResolutionHelper>();
            _sceneData.OnSceneChanged += OnSceneChanged;
        }

        private void OnSceneChanged(SceneType currentScene)
        {
            if (_currentLoadOperation == null || _currentLoadOperation.isDone)
            {
                _currentLoadOperation = SceneManager.LoadSceneAsync(currentScene.TypeToName());
                _currentLoadOperation.completed += operation =>
                {
                    _resolutionHelper.SetOrthographicSize();
                    SceneManager.SetActiveScene(SceneManager.GetSceneByName(currentScene.TypeToName()));
                };
            }
        }

        public void Dispose()
        {
            _sceneData.OnSceneChanged -= OnSceneChanged;
        }
    }
}