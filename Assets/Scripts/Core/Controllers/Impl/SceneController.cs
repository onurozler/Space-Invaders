using UnityEngine.SceneManagement;

namespace Core.Controllers.Impl
{
    public class SceneController : ISceneController
    {
        public void Load(string sceneName)
        {
            var loadSceneAsync = SceneManager.LoadSceneAsync(sceneName);
            loadSceneAsync.completed += operation =>
            {
                SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
            };
        }
    }
}