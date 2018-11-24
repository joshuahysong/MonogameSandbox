using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StellarOps.Contracts;
using System;
using System.Collections.Generic;

namespace StellarOps.Ships
{
    public class ShipCore : Entity, IFocusable
    {
        public Texture2D InteriorImage;
        public float Thrust;
        public float TurnRate;
        public float MaxVelocity;
        public List<Weapon> Weapons;

        protected int[,] tileMap;
        protected Texture2D testTile;

        Vector2 acceleration;

        public ShipCore()
        {
            testTile = MainGame.Instance.DrawTileRectangle(20, Color.Red, Color.Blue);
        }

        public override void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Position += Velocity * deltaTime;
            Velocity += acceleration * deltaTime;
            if (Velocity.X > MaxVelocity || Velocity.X < MaxVelocity * -1)
            {
                Velocity.X = Velocity.X < 0 ? -1 * MaxVelocity : MaxVelocity;
            }
            if (Velocity.Y > MaxVelocity || Velocity.Y < MaxVelocity * -1)
            {
                Velocity.Y = Velocity.Y < 0 ? -1 * MaxVelocity : MaxVelocity;
            }
            acceleration.X = 0;
            acceleration.Y = 0;
        }

        public void HandleInput()
        {
            // Apply thrust
            if (Input.IsKeyPressed(Keys.W) || Input.IsKeyPressed(Keys.Up))
            {
                acceleration.X += Thrust * (float)Math.Cos(Heading);
                acceleration.Y += Thrust * (float)Math.Sin(Heading);
            }
            // Rotate Counter-Clockwise
            if (Input.IsKeyPressed(Keys.A) || Input.IsKeyPressed(Keys.Left))
            {
                RotateCounterClockwise();
            }
            // Rotate Clockwise
            else if (Input.IsKeyPressed(Keys.D) || Input.IsKeyPressed(Keys.Right))
            {
                RotateClockwise();
            }
            // Rotate to face retro thurst heading
            else if (Input.IsKeyPressed(Keys.S) || Input.IsKeyPressed(Keys.Down))
            {
                RotateToRetro(false);
            }
            // Rotate to face retro thurst heading and thrust to brake
            else if (Input.IsKeyPressed(Keys.X))
            {
                RotateToRetro(true);
            }
            // Fire Primary Weapon
            //if (Input.IsKeyPressed(Keys.Space))
            //{
            //    PrimaryWeapon.Fire(Heading, Velocity, Position);
            //}
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Vector2 imageCenter = new Vector2(Image.Width / 2, Image.Height / 2);
            spriteBatch.Draw(MainGame.Camera.Scale > 1.2 ? InteriorImage : Image, Position, null, Color.White, Heading, imageCenter, 0.5f, SpriteEffects.None, 1f);

            // Test Tilemap

            int numberOfTilesY = tileMap.GetLength(0);
            int numberOfTilesX = tileMap.GetLength(1);
            //tilePosition.X = (int)Math.Floor(Camera.Position.X / TileSize);
            //tilePosition.Y = (int)Math.Floor(Camera.Position.Y / TileSize);

            //startLocation.X = startLocation.X - TileSize;

            //int minX = numberOfTilesX / 2 * ;
            //int maxX = (int)Math.Ceiling(tilePosition.X + (double)numberOfTilesX / 2);
            //int minY = (int)Math.Floor(tilePosition.Y - (double)numberOfTilesY / 2);
            //int maxY = (int)Math.Ceiling(tilePosition.Y + (double)numberOfTilesY / 2);

            for (int y = 0; y < tileMap.GetLength(0); y++)
            {
                for (int x = 0; x < tileMap.GetLength(1); x++)
                {
                    spriteBatch.Draw(testTile, Position, null, Color.White, Heading, imageCenter, 0.5f, SpriteEffects.None, 1f);
                }
            }
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
            float movementHeading = (float)Math.Atan2(Velocity.Y, Velocity.X);
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
            double brakingRange = MaxVelocity / 100;
            return Velocity.X < brakingRange && Velocity.X > -brakingRange && Velocity.Y < brakingRange && Velocity.Y > -brakingRange;
        }
    }
}
