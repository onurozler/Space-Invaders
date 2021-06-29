using Core.Managers.Enemy;
using UnityEngine;

namespace Helpers
{
    [ExecuteAlways]
    public class ResolutionHelper : MonoBehaviour
    {
        [SerializeField] 
        private Camera mainCamera;

        [SerializeField] 
        private float targetRatio = 1080 / 1920f;

        private void Update()
        {
            //Debug.Log( mainCamera.WorldToScreenPoint(FindObjectOfType<EnemiesManager>().transform.position));
           

            /*var currentRatio = Screen.width / (float)Screen.height;
            var scaleHeight = currentRatio / targetRatio;

            if (scaleHeight < 1.0f)
            {
                Rect rect = mainCamera.rect;

                rect.width = 1.0f;
                rect.height = scaleHeight;
                rect.x = 0;
                rect.y = (1.0f - scaleHeight) / 2.0f;

                mainCamera.rect = rect;
            }
            else 
            {
                var scaleWidth = 1.0f / scaleHeight;

                Rect rect = mainCamera.rect;

                rect.width = scaleWidth;
                rect.height = 1.0f;
                rect.x = (1.0f - scaleWidth) / 2.0f;
                rect.y = 0;

                mainCamera.rect = rect;
            }*/
        }
    }
}
