using UnityEngine;

namespace Core.Models
{
    public static class Constants
    {
        public static class Scene
        {
            public const string MenuName = "MenuScene";
            public const string GameName = "GameScene";
        }

        public static class Game
        {
            public static readonly Vector2Int Grid = new Vector2Int(11,5);
            public const int PlayerLayer = 9;
            public const int EnemyLayer = 8;
            public const float BulletSpeed = 7f;
            public const int ShieldCount = 3;
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