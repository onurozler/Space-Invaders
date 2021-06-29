using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core.Controllers.Impl
{
    public class SceneController : ISceneController
    {
        private AsyncOperation _currentLoadOperation;
            
        public SceneController()
        {
        }
        
        public void Load(string sceneName)
        {
            if (_currentLoadOperation == null || _currentLoadOperation.isDone)
            {
                _currentLoadOperation = SceneManager.LoadSceneAsync(sceneName);
                _currentLoadOperation.completed += operation =>
                {
                    SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
                };
            }
        }
    }
}