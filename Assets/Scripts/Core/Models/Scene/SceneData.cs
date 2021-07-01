using System;

namespace Core.Models.Scene
{
    public class SceneData
    {
        public event Action<SceneType> OnSceneChanged;
        
        public SceneType CurrentScene
        {
            get => _currentScene;
            set
            {
                _currentScene = value;
                OnSceneChanged?.Invoke(_currentScene);
            }
        }

        private SceneType _currentScene;
        
    }

    public enum SceneType
    {
        Menu,
        Game
    }
}