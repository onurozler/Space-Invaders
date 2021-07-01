using System;
using UnityEngine;

namespace Helpers.Scene
{
    public class SceneStateHandler : MonoBehaviour, ISceneStateHandler
    {
        public event Action OnScenePaused;
        public event Action OnSceneResumed;
        public event Action OnUpdated;

        private bool _isPaused;
        
        public void PauseOrResume()
        {
            if (!_isPaused)
            {
                _isPaused = true;
                Time.timeScale = 0;
                OnScenePaused?.Invoke();
            }
            else
            {
                _isPaused = false;
                Time.timeScale = 1;
                OnSceneResumed?.Invoke();
            }
        }
        
        private void Update()
        {
            OnUpdated?.Invoke();
        }
    }
}