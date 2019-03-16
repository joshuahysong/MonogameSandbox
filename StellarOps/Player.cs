using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StellarOps.Contracts;
using StellarOps.Ships;
using System;
using System.Collections.Generic;

namespace StellarOps
{
    public class Player : Entity, IFocusable
    {
        public Vector2 WorldPosition { get; set; }
        public float MaxSpeed => 50f;

        private float _currentSpeed;

        public Texture2D Bounds { get; set; }

        public Player()
        {
            Image = Art.Player;
            Radius = (float)Math.Ceiling((double)Image.Width / 2) + 1;
            Position = Vector2.Zero;
            Bounds = Art.CreateCircle((int)Radius - 1, Color.Green * 0.5f);
            _currentSpeed = MaxSpeed;
        }

        public override void Update(GameTime gameTime, Matrix parentTransform)
        {
            HandleInput(gameTime, parentTransform);

            if (MainGame.IsDebugging)
            {
                MainGame.Instance.PlayerDebugEntries["Position"] = $"{Math.Round(Position.X)}, {Math.Round(Position.Y)}";
                MainGame.Instance.PlayerDebugEntries["Heading"] = $"{Math.Round(Heading, 2)}";
                MainGame.Instance.PlayerDebugEntries["Speed"] = $"{_currentSpeed}";
                Vector2 playerRelativePosition = Position + Parent.ImageCenter;
                MainGame.Instance.PlayerDebugEntries["Container Tile"] = $"{Math.Floor(playerRelativePosition.X / ((ShipCore)Parent).ShipTileSize)}, {Math.Floor(playerRelativePosition.Y / ((ShipCore)Parent).ShipTileSize)}";
            }
        }

        public void HandleInput(GameTime gameTime, Matrix parentTransform)
        {
            Matrix globalTransform = LocalTransform * parentTransform;

            DecomposeMatrix(ref globalTransform, out Vector2 position, out float rotation, out Vector2 scale);
            WorldPosition = position;

            if (MainGame.Camera.Focus == this)
            {
                Vector2 targetPosition = Vector2.Transform(Input.MouseState.Position.ToVector2(), Matrix.Invert(MainGame.Camera.Transform));
                float parentRotation = 0;

                if (!float.IsNaN(targetPosition.X) && !float.IsNaN(targetPosition.X))
                {
                    Vector2 direction = Vector2.Normalize(targetPosition - position);

                    DecomposeMatrix(ref parentTransform, out Vector2 parentPosition, out parentRotation, out Vector2 parentScale);
                    Heading = (float)Math.Atan2(direction.Y, direction.X) - parentRotation;

                    if (Input.IsKeyPressed(Keys.W) || Input.IsKeyPressed(Keys.A) || Input.IsKeyPressed(Keys.S) || Input.IsKeyPressed(Keys.D))
                    {
                        Vector2 moveDirection = new Vector2();
                        float relativeHeading = 0;
                        _currentSpeed = MaxSpeed;
                        if (Input.IsKeyPressed(Keys.LeftShift))
                        {
                            _currentSpeed = (float)(MaxSpeed * 1.75);
                        }
                        if (Input.IsKeyPressed(Keys.W))
                        {
                            relativeHeading = (float)Math.Atan2(1, 0) - parentRotation;
                            Vector2 newMoveDirection = Vector2.Normalize(new Vector2((float)Math.Cos(relativeHeading), (float)Math.Sin(relativeHeading)));
                            Vector2 newMovement = newMoveDirection * (float)gameTime.ElapsedGameTime.TotalSeconds * _currentSpeed;
                            if (!IsMovingTowardsCollision(newMovement))
                            {
                                moveDirection += newMoveDirection;
                            }
                        }
                        if (Input.IsKeyPressed(Keys.S))
                        {
                            relativeHeading = (float)Math.Atan2(-1, 0) - parentRotation;
                            Vector2 newMoveDirection = Vector2.Normalize(new Vector2((float)Math.Cos(relativeHeading), (float)Math.Sin(relativeHeading)));
                            Vector2 newMovement = newMoveDirection * (float)gameTime.ElapsedGameTime.TotalSeconds * _currentSpeed;
                            if (!IsMovingTowardsCollision(newMovement))
                            {
                                moveDirection += newMoveDirection;
                            }
                        }
                        if (Input.IsKeyPressed(Keys.A))
                        {
                            relativeHeading = (float)Math.Atan2(0, 1) - parentRotation;
                            Vector2 newMoveDirection = Vector2.Normalize(new Vector2((float)Math.Cos(relativeHeading), (float)Math.Sin(relativeHeading)));
                            Vector2 newMovement = newMoveDirection * (float)gameTime.ElapsedGameTime.TotalSeconds * _currentSpeed;
                            if (!IsMovingTowardsCollision(newMovement))
                            {
                                moveDirection += newMoveDirection;
                            }
                        }
                        if (Input.IsKeyPressed(Keys.D))
                        {
                            relativeHeading = (float)Math.Atan2(0, -1) - parentRotation;
                            Vector2 newMoveDirection = Vector2.Normalize(new Vector2((float)Math.Cos(relativeHeading), (float)Math.Sin(relativeHeading)));
                            Vector2 newMovement = newMoveDirection * (float)gameTime.ElapsedGameTime.TotalSeconds * _currentSpeed;
                            if (!IsMovingTowardsCollision(newMovement))
                            {
                                moveDirection += newMoveDirection;
                            }
                        }
                        Position -= moveDirection * (float)gameTime.ElapsedGameTime.TotalSeconds * _currentSpeed;
                    }
                }
                if (Input.IsKeyToggled(Keys.F) && !Input.ManagedKeys.Contains(Keys.F))
                {
                    Input.ManagedKeys.Add(Keys.F);
                    MainGame.Camera.Focus = MainGame.Ship;
                    if (MainGame.Camera.Scale > 1f)
                    {
                        MainGame.Camera.Scale = 1F;
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
            spriteBatch.Draw(Image, position, null, Color.White, rotation - (float)(Math.PI * 0.5f), ImageCenter, scale, SpriteEffects.None, 0.0f);

            if (MainGame.IsDebugging)
            {
                var origin = new Vector2(Bounds.Width / 2, Bounds.Height / 2);
                spriteBatch.Draw(Bounds, position, null, Color.White, rotation - (float)(Math.PI * 0.5f), origin, scale, SpriteEffects.None, 0.0f);
            }
        }

        private bool IsMovingTowardsCollision(Vector2 newMovement)
        {
            Vector2 futurePosition = Position - newMovement;
            Vector2 playerRelativePosition = futurePosition + Parent.ImageCenter;
            ShipCore parent = (ShipCore)Parent;

            int tileX = (int)Math.Floor(playerRelativePosition.X / parent.ShipTileSize);
            int tileY = (int)Math.Floor(playerRelativePosition.Y / parent.ShipTileSize);

            for (int y = tileY - 1; y <= tileY + 1; y++)
            {
                for (int x = tileX - 1; x <= tileX + 1; x++)
                {
                    if (x >= 0 && y >= 0)
                    {
                        if (parent.GetCollision(x, y)
                            && IsTileCollided(futurePosition, parent.GetTileRectangle(x, y)))
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        private bool IsTileCollided(Vector2 newPosition, Rectangle tile)
        {
            float halfWidth = tile.Width / 2;
            float halfHeight = tile.Height / 2;
            float distanceX = Math.Abs((float)Math.Round(newPosition.X) - (tile.Left + halfWidth));
            float distanceY = Math.Abs((float)Math.Round(newPosition.Y) - (tile.Top + halfHeight));

            if (distanceX >= Radius + halfWidth || distanceY >= Radius + halfHeight)
            {
                return false;
            }
            if (distanceX < halfWidth || distanceY < halfHeight)
            {
                return true;
            }

            // get the distance to the corner
            distanceX -= halfWidth;
            distanceY -= halfHeight;

            // Find distance to corner and compare to circle radius
            if (distanceX * distanceX + distanceY * distanceY < Radius * Radius)
            {
                return true;
            }
            return false;
        }
    }
}
