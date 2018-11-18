using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StellarOps
{
    public class Bullet
    {
        public Texture2D texture;

        public Vector2 Position;
        public Vector2 Velocity;
        public Vector2 Origin;

        public bool IsVisible;

        public Bullet(Texture2D newTexture)
        {
            texture = newTexture;
            IsVisible = false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Position, null, Color.White, 0f, Origin, 1f, SpriteEffects.None, 1);
        }
    }
}
