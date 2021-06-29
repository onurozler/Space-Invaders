namespace Core.Models.Game.Input
{
    public class MobileInputData : IGameInputData
    {
        public InputState FirstInput { get; }
        public InputState SecondInput { get; }
    }
}