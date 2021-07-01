using UnityEngine;

namespace Core.Models.Game
{
    public class ScreenData
    {
        private readonly Camera _camera;
        
        public ScreenData(Camera camera)
        {
            _camera = camera;
        }

        public ScreenBoundary GetHeightBoundary()
        {
            var screenBoundary = new ScreenBoundary();
            screenBoundary.Min = _camera.ViewportToWorldPoint(new Vector2(0, 0f)).y;
            screenBoundary.Max = _camera.ViewportToWorldPoint(new Vector2(0, 1f)).y;

            return screenBoundary;
        }

        public ScreenBoundary GetWidthBoundary()
        {
            var screenBoundary = new ScreenBoundary();
            screenBoundary.Min = _camera.ViewportToWorldPoint(new Vector2(0, 0.5f)).x;
            screenBoundary.Max = _camera.ViewportToWorldPoint(new Vector2(1, 0.5f)).x;

            return screenBoundary;
        }

        public Vector2 GetMiddlePoint()
        {
            return _camera.ViewportToWorldPoint(new Vector2(0.5f, 0.5f));
        }
    }

    public struct ScreenBoundary
    {
        public float Max;
        public float Min;
    }
}