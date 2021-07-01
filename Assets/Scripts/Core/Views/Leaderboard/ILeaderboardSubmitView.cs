using System;

namespace Core.Views.Leaderboard
{
    public interface ILeaderboardSubmitView
    {
        void Activate();
        event Action<string> OnSubmitButtonPressed; 
    }
}