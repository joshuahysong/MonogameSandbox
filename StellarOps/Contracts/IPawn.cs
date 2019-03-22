using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StellarOps.Contracts
{
    public interface IPawn
    {
        IContainer Container { get; set; }

        Texture2D Image { get; set; }

        Vector2 Center { get; }

        float Radius { get; set; }

        void Update(GameTime gameTime, Matrix parentTransform);

        void Draw(SpriteBatch spriteBatch, Matrix parentTransform);
    }
}
