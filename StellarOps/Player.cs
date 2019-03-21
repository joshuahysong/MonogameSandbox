using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StellarOps.Contracts;
using System;

namespace StellarOps
{
    public class Player : Entity, IPawn, IFocusable
    {
        public Vector2 WorldPosition { get; set; }
        public float Radius { get; set; }
        public IContainer Container { get; set; }

        private float MaxSpeed = 50f;

        private float _currentSpeed;

        public Player()
        {
            Image = Art.Player;
            Radius = 7;//(float)Math.Ceiling((double)Image.Width / 2) + 1;
            Position = new Vector2(140,-10);
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
                Vector2 playerRelativePosition = Position + Container.ImageCenter;
                MainGame.Instance.PlayerDebugEntries["Container Tile"] = $"{Math.Floor(playerRelativePosition.X / MainGame.TileSize)}, {Math.Floor(playerRelativePosition.Y / MainGame.TileSize)}";
            }
        }

        public void HandleInput(GameTime gameTime, Matrix parentTransform)
        {
            Matrix globalTransform = LocalTransform * parentTransform;

            DecomposeMatrix(ref globalTransform, out Vector2 position, out float rotation, out Vector2 scale);
            WorldPosition = position;

            if (MainGame.Camera.Focus == this)
            {
                Vector2 targetPosition = Input.WorldMousePosition;
                float parentRotation = 0;

                if (!float.IsNaN(targetPosition.X) && !float.IsNaN(targetPosition.X))
                {
                    Vector2 direction = Vector2.Normalize(targetPosition - position);

                    DecomposeMatrix(ref parentTransform, out Vector2 parentPosition, out parentRotation, out Vector2 parentScale);
                    Heading = direction.ToAngle() - parentRotation;

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
                    Container.UseTile(Position);
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch, Matrix parentTransform)
        {
            // Calculate global transform
            Matrix globalTransform = LocalTransform * parentTransform;

            // Get values from GlobalTransform for SpriteBatch and render sprite
            DecomposeMatrix(ref globalTransform, out Vector2 position, out float rotation, out Vector2 scale);
            spriteBatch.Draw(Image, position, null, Color.White, rotation - (float)(Math.PI * 0.5f), ImageCenter, scale * 0.07f, SpriteEffects.None, 0.0f);

            if (MainGame.Camera.Focus == this)
            {
                string promptText = Container.GetUsePrompt(Position);
                if (!string.IsNullOrWhiteSpace(promptText))
                {
                    Vector2 textSize = Art.DebugFont.MeasureString(promptText);
                    Vector2 textLocation = new Vector2(position.X - textSize.X / 2, position.Y + 10 + Radius);
                    spriteBatch.Draw(Art.Pixel, new Rectangle((int)textLocation.X - 3, (int)textLocation.Y - 3, (int)textSize.X + 6, (int)textSize.Y + 6), Color.DarkCyan * 0.9f);
                    spriteBatch.DrawString(Art.DebugFont, promptText, textLocation, Color.White);
                }
            }
        }

        private bool IsMovingTowardsCollision(Vector2 newMovement)
        {
            Vector2 futurePosition = Position - newMovement;
            Tile currentTile = Container.GetTile(futurePosition);

            for (int y = currentTile.Location.Y - 1; y <= currentTile.Location.Y + 1; y++)
            {
                for (int x = currentTile.Location.X - 1; x <= currentTile.Location.X + 1; x++)
                {
                    Tile tile = Container.GetTile(new Point(x, y));
                    if (tile.Collidable && IsTileCollided(futurePosition, tile.Bounds))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private bool IsTileCollided(Vector2 newPosition, Rectangle tileBounds)
        {
            float halfWidth = tileBounds.Width / 2;
            float halfHeight = tileBounds.Height / 2;
            float distanceX = Math.Abs((float)Math.Round(newPosition.X) - (tileBounds.Left + halfWidth));
            float distanceY = Math.Abs((float)Math.Round(newPosition.Y) - (tileBounds.Top + halfHeight));

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
