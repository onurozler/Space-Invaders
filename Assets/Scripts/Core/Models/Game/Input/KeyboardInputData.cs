using UnityEngine;
using UnityInput = UnityEngine.Input;

namespace Core.Models.Game.Input
{
    public class KeyboardInputData : IGameInputData
    {
        public InputState FirstInput
        {
            get
            {
                if (UnityInput.GetKeyDown(KeyCode.P))
                {
                    return InputState.Pause;
                }
                if (UnityInput.GetKeyDown(KeyCode.Escape))
                {
                    return InputState.Quit;
                }
                if (UnityInput.GetKey(KeyCode.LeftArrow))
                {
                    return InputState.Left;
                }
                if (UnityInput.GetKey(KeyCode.RightArrow))
                {
                    return InputState.Right;
                }

                return InputState.None;
            }
        }

        public InputState SecondInput => UnityInput.GetKey(KeyCode.Space) ? InputState.Shoot : InputState.None;
    }

}