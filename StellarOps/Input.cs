using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace StellarOps
{
    public static class Input
    {
        public static List<Keys> ManagedKeys { get; set; }
        public static Vector2 ScreenMousePosition { get; private set; }
        public static Vector2 WorldMousePosition { get; private set; }
        public static int MouseScrollWheelValue { get; private set; }

        private static MouseState MouseState;
        private static KeyboardState KeyboardState;
        private static KeyboardState LastKeyboardState;
        private static MouseState LastMouseState;
        private static GamePadState GamepadState;
        private static GamePadState LastGamepadState;

        public static void Update()
        {
            ManagedKeys = new List<Keys>();
            LastKeyboardState = KeyboardState;
            LastMouseState = MouseState;
            LastGamepadState = GamepadState;

            KeyboardState = Keyboard.GetState();
            MouseState = Mouse.GetState();
            GamepadState = GamePad.GetState(PlayerIndex.One);

            ScreenMousePosition = new Vector2(MouseState.X, MouseState.Y);
            WorldMousePosition = Vector2.Transform(MouseState.Position.ToVector2(), Matrix.Invert(MainGame.Camera.Transform));
            MouseScrollWheelValue = MouseState.ScrollWheelValue;
        }

        public static bool IsKeyPressed(Keys key)
        {
            return KeyboardState.IsKeyDown(key);
        }

        public static bool WasKeyPressed(Keys key)
        {
            return LastKeyboardState.IsKeyUp(key) && KeyboardState.IsKeyDown(key);
        }

        public static bool IsKeyToggled(Keys key)
        {
            return !LastKeyboardState.IsKeyDown(key) && KeyboardState.IsKeyDown(key);
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
