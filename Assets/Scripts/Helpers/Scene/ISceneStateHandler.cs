using System;

namespace Helpers.Scene
{
    public interface ISceneStateHandler
    {
        bool IsPaused { get; }
        
        event Action OnScenePaused;
        event Action OnSceneResumed;
        event Action OnUpdated;
        event Action OnUpdatedIgnoringPause;
        
        void PauseOrResume();
    }
}