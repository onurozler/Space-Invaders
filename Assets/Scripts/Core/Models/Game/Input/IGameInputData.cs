namespace Core.Models.Game.Input
{
    public interface IGameInputData
    {
        InputState FirstInput { get; }
        InputState SecondInput { get; }
    }
    
    public enum InputState
    {
        None,
        Quit,
        Pause,
        Left,
        Right,
        Shoot
    }
}