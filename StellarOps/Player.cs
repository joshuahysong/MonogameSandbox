using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using StellarOps.Contracts;

namespace StellarOps
{
    public class Player : IFocusable
    {
        public Texture2D Ship { get; set; }
        public Vector2 Position { get; set; }
        public float Heading { get; set; }
        public float Thrust { get; set; }
        public float TurnRate { get; set; }
        public Vector2 Velocity { get; set; }
        Vector2 acceleration;

        public void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Position += Velocity * deltaTime;
            Velocity += acceleration * deltaTime;
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
                if (Velocity.X != 0 || Velocity.Y != 0)
                {
                    var movementHeading = (float)Math.Atan2(Velocity.Y, Velocity.X);
                    float retroHeading = movementHeading < 0 ? movementHeading + (float)Math.PI : movementHeading - (float)Math.PI;
                    if (Heading != retroHeading)
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
                }
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
    }
}
