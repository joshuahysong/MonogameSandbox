using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StellarOps.Weapons
{
    public class WeaponBase : Entity
    {
        public Texture2D Image { get; set; }

        public Vector2 Center => Image == null ? Vector2.Zero : new Vector2(Image.Width / 2, Image.Height / 2);

        public override void Update(GameTime gameTime, Matrix parentTransform)
        {
        }


        public override void Draw(SpriteBatch spriteBatch, Matrix parentTransform)
        {
            // Calculate global transform
            Matrix globalTransform = LocalTransform * parentTransform;

            // Get values from GlobalTransform for SpriteBatch and render sprite
            DecomposeMatrix(ref globalTransform, out Vector2 position, out float rotation, out Vector2 scale);
            spriteBatch.Draw(Image, position, null, Color.White, rotation - (float)(Math.PI * 0.5f), Center, scale * MainGame.PawnScale, SpriteEffects.None, 0.0f);
        }
    }
}
