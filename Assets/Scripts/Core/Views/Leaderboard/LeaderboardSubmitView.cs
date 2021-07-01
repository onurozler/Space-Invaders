using System;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Views.Leaderboard
{
    public class LeaderboardSubmitView : MonoBehaviour, ILeaderboardSubmitView
    {
        public event Action<string> OnSubmitButtonPressed; 
        
        [SerializeField] 
        private Text nameText;

        public void OnSubmitPressed()
        {
            OnSubmitButtonPressed?.Invoke(nameText.text);
        }
        
        public void Activate()
        {
            gameObject.SetActive(true);
        }
    }
}
