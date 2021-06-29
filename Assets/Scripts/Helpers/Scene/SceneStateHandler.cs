using System;
using UnityEngine;

namespace Helpers.Scene
{
    public class SceneStateHandler : MonoBehaviour, ISceneStateHandler
    {
        public bool IsPaused => _isPaused;
        public event Action OnScenePaused;
        public event Action OnSceneResumed;
        public event Action OnUpdated;
        public event Action OnUpdatedIgnoringPause;

        private bool _isPaused;
        
        public void PauseOrResume()
        {
            if (!_isPaused)
            {
                _isPaused = true;
                OnScenePaused?.Invoke();
            }
            else
            {
                _isPaused = false;
                OnSceneResumed?.Invoke();
            }
        }
        
        private void Update()
        {
            OnUpdatedIgnoringPause?.Invoke();
            
            if(_isPaused)
                return;
            
            OnUpdated?.Invoke();
        }
    }
}