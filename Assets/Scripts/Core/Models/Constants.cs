using UnityEngine;

namespace Core.Models
{
    public static class Constants
    {
        public static class Scene
        {
            public static string MenuName = "MenuScene";
            public static string GameName = "GameScene";
        }

        public static class Game
        {
            public static readonly Vector2Int Grid = new Vector2Int(11,5);
        }

        public static class Enemy
        {
            public static readonly Color[] Colors = 
            {
                Color.blue, Color.green, Color.yellow, Color.red
            };
        }
    }
}