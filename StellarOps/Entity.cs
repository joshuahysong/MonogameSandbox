using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StellarOps
{
    public abstract class Entity
    {
        protected Texture2D Image;
        protected Color color = Color.White;

        public Vector2 Position { get; set; }
        public Vector2 Velocity;
        public float Heading;
        public float Radius = 20;
        public bool IsExpired;

        public Vector2 Size
        {
            get
            {
                return Image == null ? Vector2.Zero : new Vector2(Image.Width, Image.Height);
            }
        }

        public abstract void Update(GameTime gameTime);

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Image, Position, null, color, Heading, Size / 2f, 1f, 0, 0);
        }
    }
}
