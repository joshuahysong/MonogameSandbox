using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TestFlying
{
    public class Camera
    {
        public Matrix Transform;
        Viewport viewport;
        Vector2 center;

        public Camera(Viewport viewport)
        {
            this.viewport = viewport;
        }

        public void Update(GameTime gameTime, MainGame ship)
        {
            center = new Vector2(ship.position.X + (ship.shipRectangle.Width / 2) - ship.GraphicsDevice.Viewport.Bounds.Width / 2,
                ship.position.Y + (ship.shipRectangle.Height / 2) - ship.GraphicsDevice.Viewport.Bounds.Height / 2);
            Transform = Matrix.CreateScale(new Vector3(1, 1, 0)) * Matrix.CreateTranslation(new Vector3(-center.X, -center.Y, 0));
        }
    }
}
