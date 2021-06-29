using UnityEngine;

namespace Core.Models.Ship
{
    [CreateAssetMenu(fileName = "ShipAssetData", menuName = "Space Invaders/Create Ship AssetData", order = 0)]
    public class ShipAssetData : ScriptableObject
    {
        public Sprite Sprite;
        public ShipType ShipType;
        public Color Color;
        public int Score;
        public float Speed;
    }

    public enum ShipType
    {
        Mystery,
        Ship1,
        Ship2,
        Ship3
    }
}