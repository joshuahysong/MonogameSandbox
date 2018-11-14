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

        public void Update(Player player)
        {
            center = new Vector2(player.position.X + (player.ship.Width / 2) - viewport.Bounds.Width / 2,
                player.position.Y + (player.ship.Height / 2) - viewport.Bounds.Height / 2);
            Transform = Matrix.CreateScale(new Vector3(1, 1, 0)) * Matrix.CreateTranslation(new Vector3(-center.X, -center.Y, 0));
        }
    }
}
