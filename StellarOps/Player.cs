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
            Image = Art.TestShip;
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
            //if (IsDead)
            //{
            //    framesUntilRespawn--;
            //    return;
            //}
            //float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            //Position += Velocity * deltaTime;
            //Velocity += acceleration * deltaTime;
            //if (Velocity.X > maxVelocity || Velocity.X < maxVelocity * -1)
            //{
            //    Velocity.X = Velocity.X < 0 ? -1 * maxVelocity : maxVelocity;
            //}
            //if (Velocity.Y > maxVelocity || Velocity.Y < maxVelocity * -1)
            //{
            //    Velocity.Y = Velocity.Y < 0 ? -1 * maxVelocity : maxVelocity;
            //}
            //acceleration.X = 0;
            //acceleration.Y = 0;
            Ship.Update(gameTime);
        }

        public void HandleInput()
        {
            if (IsPiloting)
            {
                Ship.HandleInput();
            }
        //    // Apply thrust
        //    if (Input.IsKeyPressed(Keys.W) || Input.IsKeyPressed(Keys.Up))
        //    {
        //        acceleration.X += Thrust * (float)Math.Cos(Heading);
        //        acceleration.Y += Thrust * (float)Math.Sin(Heading);
        //    }
        //    // Rotate Counter-Clockwise
        //    if (Input.IsKeyPressed(Keys.A) || Input.IsKeyPressed(Keys.Left))
        //    {
        //        RotateCounterClockwise();
        //    }
        //    // Rotate Clockwise
        //    else if (Input.IsKeyPressed(Keys.D) || Input.IsKeyPressed(Keys.Right))
        //    {
        //        RotateClockwise();
        //    }
        //    // Rotate to face retro thurst heading
        //    else if (Input.IsKeyPressed(Keys.S) || Input.IsKeyPressed(Keys.Down))
        //    {
        //        RotateToRetro(false);
        //    }
        //    // Rotate to face retro thurst heading and thrust to brake
        //    else if (Input.IsKeyPressed(Keys.X))
        //    {
        //        RotateToRetro(true);
        //    }
        //    // Fire Primary Weapon
        //    if (Input.IsKeyPressed(Keys.Space))
        //    {
        //        PrimaryWeapon.Fire(Heading, Velocity, Position);
        //    }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //Vector2 imageCenter = new Vector2(Image.Width / 2, Image.Height / 2);
            //spriteBatch.Draw(MainGame.Camera.Scale > 1.2 ? Art.TestShipInterior : Image, Position, null, Color.White, Heading/* + (float)Math.PI / 2*/, imageCenter, 0.5f, SpriteEffects.None, 1f);
            Ship.Draw(spriteBatch);
        }

        //private void RotateClockwise()
        //{
        //    Heading += TurnRate;
        //    if (Heading > Math.PI)
        //    {
        //        Heading = (float)-Math.PI;
        //    }
        //}

        //private void RotateCounterClockwise()
        //{
        //    Heading -= TurnRate;
        //    if (Heading < -Math.PI)
        //    {
        //        Heading = (float)Math.PI;
        //    }
        //}

        //private void RotateToRetro(bool IsBraking)
        //{
        //    float movementHeading = (float)Math.Atan2(Velocity.Y, Velocity.X);
        //    float retroHeading = movementHeading < 0 ? movementHeading + (float)Math.PI : movementHeading - (float)Math.PI;
        //    if (Heading != retroHeading && !IsWithinBrakingRange())
        //    {
        //        double retroDegrees = (retroHeading + Math.PI) * (180.0 / Math.PI);
        //        double headingDegrees = (Heading + Math.PI) * (180.0 / Math.PI);
        //        double turnRateDegrees = Math.PI * 2 * TurnRate / 100 * 360 * 2;
        //        double retroOffset = headingDegrees < retroDegrees ? (headingDegrees + 360) - retroDegrees : headingDegrees - retroDegrees;

        //        if (retroOffset >= 360 - turnRateDegrees || retroOffset <= turnRateDegrees)
        //        {
        //            Heading = retroHeading;
        //        }
        //        else
        //        {
        //            if (retroOffset < 180)
        //            {
        //                RotateCounterClockwise();
        //            }
        //            else
        //            {
        //                RotateClockwise();
        //            }
        //        }
        //    }
        //    else if (IsBraking)
        //    {
        //        if (IsWithinBrakingRange())
        //        {
        //            Velocity = Vector2.Zero;
        //        }
        //        else if (Heading == retroHeading)
        //        {
        //            acceleration.X += Thrust * (float)Math.Cos(Heading);
        //            acceleration.Y += Thrust * (float)Math.Sin(Heading);
        //        }
        //    }
        //}

        //private bool IsWithinBrakingRange()
        //{
        //    double brakingRange = maxVelocity / 100;
        //    return Velocity.X < brakingRange && Velocity.X > -brakingRange && Velocity.Y < brakingRange && Velocity.Y > -brakingRange;
        //}
    }
}
