using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StellarOps.Contracts;
using StellarOps.Ships;
using System;

namespace StellarOps
{
    public class Player : Entity, IFocusable
    {
        //public float Thrust;
        //public float TurnRate;
        //public ShipCore Ship;
        //float maxVelocity => 500;
        //Vector2 acceleration;
        //Weapon PrimaryWeapon;

        //bool IsPiloting;

        public Player()
        {
            //IsPiloting = true;
            Image = Art.Player;
            Position = new Vector2(540,180);
            //Position = Vector2.Zero;
            //PrimaryWeapon = new Weapon()
            //{
            //    Cooldown = 10,
            //    CooldownRemaining = 0,
            //    AttachedPosition = new Vector2(30, 0),
            //    Speed = 1000f,
            //    Accuracy = 99f
            //};
        }

        public override void Update(GameTime gameTime)
        {
            HandleInput(gameTime);
            //Ship.Update(gameTime);
        }

        public void HandleInput(GameTime gameTime)
        {
            //if (IsPiloting)
            //{
            //    Ship.HandleInput();
            //}

            if (Input.MouseState.LeftButton == ButtonState.Pressed/* && !Ship.InteriorIsDisplayed*/)
            {
                Vector2 targetPosition = Vector2.Transform(Input.MouseState.Position.ToVector2(), Matrix.Invert(MainGame.Camera.Transform));
                if (!float.IsNaN(targetPosition.X) && !float.IsNaN(targetPosition.X))
                {
                    Vector2 direction = Vector2.Normalize(targetPosition - Position);
                    Heading = (float)Math.Atan2(direction.Y, direction.X);

                    Position += direction * (float)gameTime.ElapsedGameTime.TotalSeconds * 50.0f;
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch, Matrix parentTransform)
        {
            if (parentTransform != Matrix.Identity || MainGame.Camera.Focus == this)
            {
                // Calculate global transform
                Matrix globalTransform = LocalTransform * parentTransform;

                //// Get values from GlobalTransform for SpriteBatch and render sprite
                Vector2 position, scale;
                float rotation;
                DecomposeMatrix(ref globalTransform, out position, out rotation, out scale);
                Console.WriteLine($"{position} | {Position}");
                spriteBatch.Draw(Image, position, null, Color.White, rotation - (float)(Math.PI * 0.5f), Vector2.Zero, scale, SpriteEffects.None, 0.0f);
            }
        }
    }
}
