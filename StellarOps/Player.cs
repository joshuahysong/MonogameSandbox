using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StellarOps.Contracts;
using System;

namespace StellarOps
{
    public class Player : Entity, IFocusable
    {
        public Player()
        {
            Image = Art.Player;
            //Position = new Vector2(Image.Width / 2, Image.Height / 2);
            Position = Vector2.Zero;
            IsChild = true;
        }

        public override void Update(GameTime gameTime, Matrix parentTransform)
        {
            HandleInput(gameTime, parentTransform);
        }

        public void HandleInput(GameTime gameTime, Matrix parentTransform)
        {

            if (Input.MouseState.LeftButton == ButtonState.Pressed)
            {
                Vector2 targetPosition = Vector2.Transform(Input.MouseState.Position.ToVector2(), Matrix.Invert(MainGame.Camera.Transform));
                if (!float.IsNaN(targetPosition.X) && !float.IsNaN(targetPosition.X))
                {
                    Matrix globalTransform = LocalTransform * parentTransform;

                    Vector2 position;
                    Vector2 scale;
                    float rotation;
                    DecomposeMatrix(ref globalTransform, out position, out rotation, out scale);
                    Vector2 direction = Vector2.Normalize(targetPosition - position);

                    // Get parent rotation
                    DecomposeMatrix(ref parentTransform, out position, out rotation, out scale);

                    Heading = (float)Math.Atan2(direction.Y, direction.X) - rotation;

                    //Position += direction * (float)gameTime.ElapsedGameTime.TotalSeconds * 50.0f;
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch, Matrix parentTransform)
        {
            // Calculate global transform
            Matrix globalTransform = LocalTransform * parentTransform;

            //// Get values from GlobalTransform for SpriteBatch and render sprite
            Vector2 position, scale;
            float rotation;
            DecomposeMatrix(ref globalTransform, out position, out rotation, out scale);
            var imageCenter = new Vector2(Image.Width / 8, Image.Height / 8);
            spriteBatch.Draw(Image, position, null, Color.White, rotation - (float)(Math.PI * 0.5f), ImageCenter, scale, SpriteEffects.None, 0.0f);
        }
    }
}
