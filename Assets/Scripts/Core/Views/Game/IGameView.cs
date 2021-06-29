
namespace Core.Views.Game
{
    public interface IGameView
    {
        void FillData(int totalTime);
        void SetScore(int score);
        void SetLives(int lives);
        void UpdateExtraBonus(float timer);
        void ShowExtraBonus(int bonusPoints);
    }
}