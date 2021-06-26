using UnityEngine;

namespace Core.Models.Game
{
    [CreateAssetMenu(fileName = "GameData", menuName = "Space Invaders/Create Game Data", order = 1)]
    public class GameData : ScriptableObject
    {
        [Header("Game Grid")]
        [Range(5,15)]
        [SerializeField] 
        private int gridX;
        
        [Range(5,15)]
        [SerializeField] 
        private int gridY;

        [Header("Extra Bonus")]
        [SerializeField] 
        private int extraBonusTotalTime;
        
        [SerializeField] 
        private int extraBonusMultiplier;
    }
}