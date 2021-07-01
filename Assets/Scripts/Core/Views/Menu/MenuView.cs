using System;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Views.Menu
{
    public class MenuView : MonoBehaviour, IMenuView
    {
        [SerializeField] private Text octopusScoreText;
        [SerializeField] private Text crabScoreText;
        [SerializeField] private Text squidScoreText;
        
        public event Action OnPlayButtonPressed;
        public event Action OnLeaderboardButtonPressed;
        
        public void FillData(int octopusScore, int crabScore, int squidScore)
        {
            octopusScoreText.text = $"= {octopusScore} POINTS";
            crabScoreText.text = $"= {crabScore} POINTS";
            squidScoreText.text = $"= {squidScore} POINTS";
        }

        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
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
