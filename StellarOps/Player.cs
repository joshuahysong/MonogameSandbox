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
        public ShipCore Ship;
        //float maxVelocity => 500;
        //Vector2 acceleration;
        //Weapon PrimaryWeapon;

        bool IsPiloting;

        public Player()
        {
            Ship = new TestShip(Vector2.Zero);
            IsPiloting = true;
            Image = Art.Player;
            Position = new Vector2(235,-18);
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
            Ship.Update(gameTime);
        }

        public void HandleInput(GameTime gameTime)
        {
            if (IsPiloting)
            {
                Ship.HandleInput();
            }

            if (Input.MouseState.LeftButton == ButtonState.Pressed && Ship.InteriorIsDisplayed)
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

        public override void Draw(SpriteBatch spriteBatch)
        {
            Ship.Draw(spriteBatch);
            if (Ship.InteriorIsDisplayed)
            {
                Vector2 imageCenter = new Vector2(Image.Width / 2, Image.Height / 2);
                spriteBatch.Draw(Image, Position, null, Color.White, Heading - (float)(Math.PI * 0.5f), imageCenter, 1f, SpriteEffects.None, 1f);
            }
        }
    }
}
