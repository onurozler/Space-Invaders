using UnityEngine;

namespace Helpers
{
    [ExecuteAlways]
    public class ResolutionHelper : MonoBehaviour
    {
        private int _resolutionX;
        private int _resolutionY;
        
        private void Awake()
        {
            _resolutionX = Screen.width;
            _resolutionY = Screen.height;
        }

        private void Update()
        {
            if (_resolutionX != Screen.width || _resolutionY != Screen.height)
            {
                _resolutionX = Screen.width;
                _resolutionY = Screen.height;
                
                SetOrthographicSize();
            }
        }

        public void SetOrthographicSize()
        {
            if ((float)_resolutionX / _resolutionY < 1)
            {
                Camera.main.orthographicSize = 15f;
            }
            else
            {
                Camera.main.orthographicSize = 7f;
            }
        }
    }
}
