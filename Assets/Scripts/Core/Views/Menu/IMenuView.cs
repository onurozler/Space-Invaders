using System;

namespace Core.Views.Menu
{
    public interface IMenuView
    {
        event Action OnPlayButtonPressed;
        event Action OnLeaderboardButtonPressed;
        void FillData();
    }
}