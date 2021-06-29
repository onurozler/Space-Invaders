using System;

namespace Core.Models.Player
{
    public class PlayerData
    {
        public event Action<int> OnLivesChanged; 
        public event Action<int> OnExtraScoreChanged;
        public event Action<int> OnScoreChanged;
        public event Action OnGameOver;

        public int ExtraScore
        {
            get => _extraScore;
            set
            {
                _extraScore = value;
                OnExtraScoreChanged?.Invoke(_extraScore);
            }
        }
        
        public int Score
        {
            get => _score;
            set
            {
                _score = value;
                OnScoreChanged?.Invoke(_score);
            }
        }
        
        public int Lives
        {
            get => _lives;
            set
            {
                _lives = value;
                OnLivesChanged?.Invoke(_lives);
            }
        }

        public PlayerState PlayerState
        {
            get => _playerState;
            set
            {
                if(value == PlayerState.GameOver)
                    OnGameOver?.Invoke();
                
                _playerState = value;
            }
        }

        private int _lives;
        private PlayerState _playerState;
        private int _score;
        private int _extraScore;

        public PlayerData()
        {
            PlayerState = PlayerState.Playing;
            Score = 0;
        }
    }

    public enum PlayerState
    {
        Playing,
        GameOver
    }
}