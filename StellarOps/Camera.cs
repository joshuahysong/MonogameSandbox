using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StellarOps.Contracts;

namespace StellarOps
{
    public class Camera
    {
        public Vector2 Position { get; set; }
        public float Rotation { get; set; }
        public Vector2 Origin { get; set; }
        public float Scale { get; set; }
        public Matrix Transform { get; set; }

        private int previousScrollValue;

        public Camera()
        {
            Scale = 1f;
        }

        public void Update(IFocusable focus)
        {
            Transform = Matrix.CreateTranslation(new Vector3(-focus.Position.X, -focus.Position.Y, 0)) *
                Matrix.CreateRotationZ(Rotation) *
                Matrix.CreateScale(new Vector3(Scale, Scale, 0)) *
                Matrix.CreateTranslation(new Vector3(MainGame.ScreenCenter.X, MainGame.ScreenCenter.Y, 0));

            Origin = MainGame.ScreenCenter / Scale;
            Position = focus.Position;
        }

        public bool IsInView(Vector2 position, Texture2D texture)
        {
            if ((position.X + texture.Width) < (Position.X - Origin.X) || (position.X) > (Position.X + Origin.X))
            {
                return false;
            }

            if ((position.Y + texture.Height) < (Position.Y - Origin.Y) || (position.Y) > (Position.Y + Origin.Y))
            {
                return false;
            }

            return true;
        }

        public void Input(KeyboardState keyboardState, MouseState mouseState)
        {
            if (mouseState.ScrollWheelValue < previousScrollValue)
            {
                Scale -= 0.1f;
                if (Scale < 0.5f)
                {
                    Scale = 0.5f;
                }
            }
            else if (mouseState.ScrollWheelValue > previousScrollValue)
            {
                Scale += 0.1f;
                if (Scale > 4f)
                {
                    Scale = 4f;
                }
            }
            previousScrollValue = mouseState.ScrollWheelValue;
        }
    }
}
