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
        public bool InteriorIsDisplayed;
        public int[,] TileMap;
        public Vector2 WorldPosition { get; set; }

        protected Dictionary<int, Texture2D> debugTiles;

        private Vector2 _acceleration;

        public ShipCore()
        {
            debugTiles = new Dictionary<int, Texture2D>
            {
                { 0, MainGame.Instance.DrawTileRectangle(35, 35, Color.DimGray * 0.2f, Color.DimGray * 0.3f) },
                { 1, MainGame.Instance.DrawTileRectangle(35, 35, Color.Blue * 0.2f, Color.Blue * 0.3f) },
                { 2, MainGame.Instance.DrawTileRectangle(35, 35, Color.Red * 0.2f, Color.Red * 0.3f) }
            };
        }

        public override void Update(GameTime gameTime, Matrix parentTransform)
        {
            HandleInput();

            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Velocity += _acceleration * deltaTime;

            if (Velocity.LengthSquared() > MaxVelocity * MaxVelocity)
            {
                Velocity.Normalize();
                Velocity *= MaxVelocity;
            }
            _acceleration.X = 0;
            _acceleration.Y = 0;
            Position += Velocity * deltaTime;
            WorldPosition = Position;

            // Interior Display
            if (MainGame.Camera.Scale > 1.2)
            {
                InteriorIsDisplayed = true;
            }
            else
            {
                InteriorIsDisplayed = false;
            }

            Children.ForEach(c => c.Update(gameTime, LocalTransform));
        }

        public void HandleInput()
        {
            if (MainGame.Camera.Focus == this)
            {
                // Apply thrust
                if (Input.IsKeyPressed(Keys.W) || Input.IsKeyPressed(Keys.Up))
                {
                    _acceleration.X += Thrust * (float)Math.Cos(Heading);
                    _acceleration.Y += Thrust * (float)Math.Sin(Heading);
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
                if (Input.IsKeyToggled(Keys.F) && !Input.ManagedKeys.Contains(Keys.F))
                {
                    Input.ManagedKeys.Add(Keys.F);
                    MainGame.Camera.Focus = MainGame.Player;
                    if (MainGame.Camera.Scale < 2f)
                    {
                        MainGame.Camera.Scale = 2F;
                    }
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch, Matrix parentTransform)
        {
            // Calculate global transform
            Matrix globalTransform = LocalTransform * parentTransform;

            // Get values from GlobalTransform for SpriteBatch and render sprite
            DecomposeMatrix(ref globalTransform, out Vector2 position, out float rotation, out Vector2 scale);
            spriteBatch.Draw(InteriorIsDisplayed ? InteriorImage : Image, position, null, Color.White, rotation, ImageCenter, scale, SpriteEffects.None, 0.0f);

            // Draw Children
            Children.ForEach(c => c.Draw(spriteBatch, globalTransform));

            //Debug Tilemap
            Vector2 imageCenter = new Vector2(Image.Width / 2, Image.Height / 2);
            Vector2 origin = imageCenter;
            if (MainGame.IsDebugging)
            {
                for (int y = 0; y < TileMap.GetLength(0); y++)
                {
                    for (int x = 0; x < TileMap.GetLength(1); x++)
                    {
                        Texture2D tileToDraw = debugTiles[TileMap[y, x]];
                        Vector2 offset = new Vector2(x * tileToDraw.Width, y * tileToDraw.Height);
                        origin = imageCenter - offset;
                        spriteBatch.Draw(tileToDraw, Position, null, Color.White, Heading, origin, 1f, SpriteEffects.None, 1f);
                    }
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
                    _acceleration.X += Thrust * (float)Math.Cos(Heading);
                    _acceleration.Y += Thrust * (float)Math.Sin(Heading);
                }
            }
        }

        private bool IsWithinBrakingRange()
        {
            double brakingRange = MaxVelocity / 100;
            return Velocity.X < brakingRange && Velocity.X > -brakingRange && Velocity.Y < brakingRange && Velocity.Y > -brakingRange;
        }

        public bool GetCollision(int x, int y)
        {
            return TileMap[y, x] == 1;
        }

        public Rectangle GetTileRectangle(int x, int y)
        {
            Vector2 imageCenter = new Vector2(Image.Width / 2, Image.Height / 2);
            Vector2 tilePosition = new Vector2(x * 35, y * 35);
            tilePosition -= imageCenter;
            return new Rectangle((int)tilePosition.X, (int)tilePosition.Y, 35, 35);
        }

        /// <summary>
        /// Gets the bounding rectangle of a tile in world space.
        /// </summary>
        public Rectangle GetBounds(int x, int y)
        {
            return new Rectangle(x * 35, y * 35, 35, 35);
        }
    }
}
