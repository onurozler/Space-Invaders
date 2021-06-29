using UnityEngine;

namespace Core.Models.Game
{
    [CreateAssetMenu(fileName = "GameAssetData", menuName = "Space Invaders/Create Game AssetData", order = 1)]
    public class GameAssetData : ScriptableObject
    {
        [Header("Extra Bonus")]
        public int ExtraBonusTotalTime;
        
        [Range(2,10)]
        public int ExtraBonusMultiplier;
    }
}