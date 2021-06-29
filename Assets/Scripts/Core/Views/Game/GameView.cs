using UnityEngine;
using UnityEngine.UI;

namespace Core.Views.Game
{
    public class GameView : MonoBehaviour, IGameView
    {
        [SerializeField] private Image[] liveImages;
        [SerializeField] private Text scoreText;
        [SerializeField] private Text bonusTimerText;
        
        public void FillData(int totalTime)
        {
            SetScore(0);
            UpdateExtraBonus(totalTime);
        }

        public void SetScore(int score)
        {
            scoreText.text = $"Score:\n{score}";
        }

        public void SetLives(int lives)
        {
            for (int i = 0; i < liveImages.Length; i++)
            {
                liveImages[i].gameObject.SetActive(lives - 1 >= i);
            }
        }

        public void UpdateExtraBonus(float timer)
        {
            bonusTimerText.text = $"Bonus TIMER:\n{timer}";
        }

        public void ShowExtraBonus(int bonusPoints)
        {
            bonusTimerText.text = $"Bonus POINTS:\n{bonusPoints}";
        }
    }
}
