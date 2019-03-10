using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

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
        public static Vector2 ScreenMousePosition;
        public static Vector2 WorldMousePosition;
        public static List<Keys> ManagedKeys;

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
            WorldMousePosition = Vector2.Transform(Input.MouseState.Position.ToVector2(), Matrix.Invert(MainGame.Camera.Transform));
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
