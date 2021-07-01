using System;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Views.Leaderboard
{
    public class LeaderboardView : MonoBehaviour, ILeaderboardView
    {
        [SerializeField] 
        private Text[] leaderboardRecords;

        public event Action OnClosedButtonPressed;

        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }

        public void AddRecord(int index, string playerName, int score)
        {
            leaderboardRecords[index].text = $"{index + 1} - {playerName} = {score}";
        }

        public void OnClose()
        {
            OnClosedButtonPressed?.Invoke();
        }
    }
}