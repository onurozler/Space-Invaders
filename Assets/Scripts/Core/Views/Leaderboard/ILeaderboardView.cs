using System;

namespace Core.Views.Leaderboard
{
    public interface ILeaderboardView
    {
        event Action OnClosedButtonPressed;
        
        void SetActive(bool isActive);
        void AddRecord(int index, string playerName, int score);
    }
}