using System;

namespace Helpers.Scene
{
    public interface ISceneStateHandler
    {
        event Action OnScenePaused;
        event Action OnSceneResumed;
        event Action OnUpdated;
        
        void PauseOrResume();
    }
}