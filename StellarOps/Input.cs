using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace StellarOps
{
    public static class Input
    {
        public static KeyboardState KeyboardState;
        public static KeyboardState LastKeyboardState;
        public static MouseState MouseState;
        public static MouseState LastMouseState;
        public static GamePadState GamepadState;
        public static GamePadState LastGamepadState;

        public static void Update()
        {
            LastKeyboardState = KeyboardState;
            LastMouseState = MouseState;
            LastGamepadState = GamepadState;

            KeyboardState = Keyboard.GetState();
            MouseState = Mouse.GetState();
            GamepadState = GamePad.GetState(PlayerIndex.One);
        }

        public static bool IsKeyPressed(Keys key)
        {
            return KeyboardState.IsKeyDown(key);
        }

        public static bool WasKeyPressed(Keys key)
        {
            return LastKeyboardState.IsKeyUp(key) && KeyboardState.IsKeyDown(key);
        }

        public static bool IsButtonPressed(Buttons button)
        {
            return GamepadState.IsButtonDown(button);
        }

        public static bool WasButtonPressed(Buttons button)
        {
            return LastGamepadState.IsButtonUp(button) && GamepadState.IsButtonDown(button);
        }
    }
}
