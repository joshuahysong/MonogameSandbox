using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StellarOps.Contracts;
using System;

namespace StellarOps
{
    public class Player : IFocusable
    {
        public Texture2D Ship { get; set; }
        public Vector2 Position { get; set; }
        public float Heading { get; set; }
        public float Thrust { get; set; }
        public float TurnRate { get; set; }
        public Vector2 Velocity;
        float maxVelocity => 500;
        Vector2 acceleration;

        const int cooldownFrames = 20;
        int cooldownRemaining = 0;
        static Random rand = new Random();

        public void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Position += Velocity * deltaTime;
            Velocity += acceleration * deltaTime;
            if (Velocity.X > maxVelocity || Velocity.X < maxVelocity * -1)
            {
                Velocity.X = Velocity.X < 0 ? -1 * maxVelocity : maxVelocity;
            }
            if (Velocity.Y > maxVelocity || Velocity.Y < maxVelocity * -1)
            {
                Velocity.Y = Velocity.Y < 0 ? -1 * maxVelocity : maxVelocity;
            }
            acceleration.X = 0;
            acceleration.Y = 0;
        }

        public void HandleInput(KeyboardState KeyState)
        {
            // Apply thrust
            if (KeyState.IsKeyDown(Keys.W) || KeyState.IsKeyDown(Keys.Up))
            {
                acceleration.X += Thrust * (float)Math.Cos(Heading);
                acceleration.Y += Thrust * (float)Math.Sin(Heading);
            }
            // Rotate Counter-Clockwise
            if (KeyState.IsKeyDown(Keys.A) || KeyState.IsKeyDown(Keys.Left))
            {
                RotateCounterClockwise();
            }
            // Rotate Clockwise
            if (KeyState.IsKeyDown(Keys.D) || KeyState.IsKeyDown(Keys.Right))
            {
                RotateClockwise();
            }
            // Rotate to face retro thurst heading
            if (KeyState.IsKeyDown(Keys.S) || KeyState.IsKeyDown(Keys.Down))
            {
                RotateToRetro(false);
            }
            if (KeyState.IsKeyDown(Keys.X))
            {
                RotateToRetro(true);
            }
            if (KeyState.IsKeyDown(Keys.Space))
            {
                FirePrimaryWeapon();
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            var shipCenter = new Vector2(Ship.Width / 2, Ship.Height / 2);
            spriteBatch.Draw(Ship, Position, null, Color.White, Heading + (float)Math.PI / 2, shipCenter, 0.5f, SpriteEffects.None, 1f);
        }

        private void RotateClockwise()
        {
            Heading += TurnRate;
            if (Heading > Math.PI)
            {
                Heading = (float)-Math.PI;
            }
        }

        private void RotateCounterClockwise()
        {
            Heading -= TurnRate;
            if (Heading < -Math.PI)
            {
                Heading = (float)Math.PI;
            }
        }

        private void RotateToRetro(bool IsBraking)
        {
            var movementHeading = (float)Math.Atan2(Velocity.Y, Velocity.X);
            float retroHeading = movementHeading < 0 ? movementHeading + (float)Math.PI : movementHeading - (float)Math.PI;
            if (Heading != retroHeading && !IsWithinBrakingRange())
            {
                double retroDegrees = (retroHeading + Math.PI) * (180.0 / Math.PI);
                double headingDegrees = (Heading + Math.PI) * (180.0 / Math.PI);
                double turnRateDegrees = Math.PI * 2 * TurnRate / 100 * 360 * 2;
                double retroOffset = headingDegrees < retroDegrees ? (headingDegrees + 360) - retroDegrees : headingDegrees - retroDegrees;

                if (retroOffset >= 360 - turnRateDegrees || retroOffset <= turnRateDegrees)
                {
                    Heading = retroHeading;
                }
                else
                {
                    if (retroOffset < 180)
                    {
                        RotateCounterClockwise();
                    }
                    else
                    {
                        RotateClockwise();
                    }
                }
            }
            else if (IsBraking)
            {
                if (IsWithinBrakingRange())
                {
                    Velocity = Vector2.Zero;
                }
                else if (Heading == retroHeading)
                {
                    acceleration.X += Thrust * (float)Math.Cos(Heading);
                    acceleration.Y += Thrust * (float)Math.Sin(Heading);
                }
            }
        }

        private bool IsWithinBrakingRange()
        {
            double brakingRange = maxVelocity / 100;
            return Velocity.X < brakingRange && Velocity.X > -brakingRange && Velocity.Y < brakingRange && Velocity.Y > -brakingRange;
        }

        private void FirePrimaryWeapon()
        {
            if (cooldownRemaining <= 0)
            {
                cooldownRemaining = cooldownFrames;
                Quaternion aimQuat = Quaternion.CreateFromYawPitchRoll(0, 0, Heading);

                float randomSpread = rand.NextFloat(-0.01f, 0.01f) + rand.NextFloat(-0.01f, 0.01f);
                Vector2 vel = MathUtil.FromPolar(Heading + randomSpread, 1000f);

                Vector2 offset = Vector2.Transform(new Vector2(30, 0), aimQuat);
                EntityManager.Add(new Bullet(Position + offset, vel + Velocity));
            }

            if (cooldownRemaining > 0)
            {
                 cooldownRemaining--;
            }
        }
    }
}
