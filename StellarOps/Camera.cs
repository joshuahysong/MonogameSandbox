using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
        public IFocusable Focus { get; set; }

        private int previousScrollValue;

        public Camera()
        {
            Scale = 1f;
        }

        public void Update(IFocusable focus)
        {
            Focus = focus;
            Transform = Matrix.CreateTranslation(new Vector3(-focus.WorldPosition.X, -focus.WorldPosition.Y, 0)) *
                Matrix.CreateRotationZ(Rotation) *
                Matrix.CreateScale(new Vector3(Scale, Scale, 1)) *
                Matrix.CreateTranslation(new Vector3(MainGame.ScreenCenter.X, MainGame.ScreenCenter.Y, 0));

            Origin = MainGame.ScreenCenter / Scale;
            Position = focus.WorldPosition;
        }

        public void HandleInput()
        {
            if (Input.MouseScrollWheelValue < previousScrollValue)
            {
                Scale -= 0.1f;
                if (Scale < 1f && Focus == MainGame.Player)
                {
                    Scale = 1f;
                }
                else if (Scale < 0.1f)
                {
                    Scale = 0.1f;
                }
            }
            else if (Input.MouseScrollWheelValue > previousScrollValue)
            {
                Scale += 0.1f;
                if (Scale > 4f)
                {
                    Scale = 4f;
                }
            }
            previousScrollValue = Input.MouseScrollWheelValue;
        }
    }
}
