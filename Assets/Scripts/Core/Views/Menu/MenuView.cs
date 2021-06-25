using System;
using UnityEngine;

namespace Core.Views.Menu
{
    public class MenuView : MonoBehaviour, IMenuView
    {
        public event Action OnPlayButtonPressed;
        public event Action OnLeaderboardButtonPressed;
        
        public void FillData()
        {
            
        }

        public void PlayButtonPressed()
        {
            OnPlayButtonPressed?.Invoke();
        }

        public void LeaderboardButtonPressed()
        {
            OnLeaderboardButtonPressed?.Invoke();
        }
    }
}
