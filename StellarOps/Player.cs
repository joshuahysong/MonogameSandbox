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
        public Vector2 WorldPosition { get; set; }
        public float Speed => 50f;

        public Player()
        {
            Image = Art.Player;
            Radius = (float)Math.Ceiling((double)Image.Width / 2) + 1;
            Position = Vector2.Zero;
        }

        public override void Update(GameTime gameTime, Matrix parentTransform)
        {
            HandleInput(gameTime, parentTransform);
        }

        public void HandleInput(GameTime gameTime, Matrix parentTransform)
        {
            Matrix globalTransform = LocalTransform * parentTransform;

            Vector2 position;
            Vector2 scale;
            float rotation;
            DecomposeMatrix(ref globalTransform, out position, out rotation, out scale);
            WorldPosition = position;

            if (MainGame.Camera.Focus == this)
            {
                Vector2 targetPosition = Vector2.Transform(Input.MouseState.Position.ToVector2(), Matrix.Invert(MainGame.Camera.Transform));
                float parentRotation = 0;

                if (!float.IsNaN(targetPosition.X) && !float.IsNaN(targetPosition.X))
                {
                    Vector2 direction = Vector2.Normalize(targetPosition - position);

                    Vector2 parentPosition;
                    Vector2 parentScale;
                    DecomposeMatrix(ref parentTransform, out parentPosition, out parentRotation, out parentScale);
                    Heading = (float)Math.Atan2(direction.Y, direction.X) - parentRotation;

                    if (Input.IsKeyPressed(Keys.W) || Input.IsKeyPressed(Keys.A) || Input.IsKeyPressed(Keys.S) || Input.IsKeyPressed(Keys.D))
                    {
                        Vector2 moveDirection = new Vector2();
                        float relativeHeading = 0;
                        float movementSpeed = Speed;
                        if (Input.IsKeyPressed(Keys.LeftShift))
                        {
                            movementSpeed = (float)(Speed * 1.75);
                        }
                        if (Input.IsKeyPressed(Keys.W))
                        {
                            relativeHeading = (float)Math.Atan2(1, 0) - parentRotation;
                            Vector2 newMoveDirection = Vector2.Normalize(new Vector2((float)Math.Cos(relativeHeading), (float)Math.Sin(relativeHeading)));
                            Vector2 newMovement = newMoveDirection * (float)gameTime.ElapsedGameTime.TotalSeconds * movementSpeed;
                            if (!NewPositionBlocked(newMovement))
                            {
                                moveDirection += newMoveDirection;
                            }
                        }
                        if (Input.IsKeyPressed(Keys.S))
                        {
                            relativeHeading = (float)Math.Atan2(-1, 0) - parentRotation;
                            Vector2 newMoveDirection = Vector2.Normalize(new Vector2((float)Math.Cos(relativeHeading), (float)Math.Sin(relativeHeading)));
                            Vector2 newMovement = newMoveDirection * (float)gameTime.ElapsedGameTime.TotalSeconds * movementSpeed;
                            if (!NewPositionBlocked(newMovement))
                            {
                                moveDirection += newMoveDirection;
                            }
                        }
                        if (Input.IsKeyPressed(Keys.A))
                        {
                            relativeHeading = (float)Math.Atan2(0, 1) - parentRotation;
                            Vector2 newMoveDirection = Vector2.Normalize(new Vector2((float)Math.Cos(relativeHeading), (float)Math.Sin(relativeHeading)));
                            Vector2 newMovement = newMoveDirection * (float)gameTime.ElapsedGameTime.TotalSeconds * movementSpeed;
                            if (!NewPositionBlocked(newMovement))
                            {
                                moveDirection += newMoveDirection;
                            }
                        }
                        if (Input.IsKeyPressed(Keys.D))
                        {
                            relativeHeading = (float)Math.Atan2(0, -1) - parentRotation;
                            Vector2 newMoveDirection = Vector2.Normalize(new Vector2((float)Math.Cos(relativeHeading), (float)Math.Sin(relativeHeading)));
                            Vector2 newMovement = newMoveDirection * (float)gameTime.ElapsedGameTime.TotalSeconds * movementSpeed;
                            if (!NewPositionBlocked(newMovement))
                            {
                                moveDirection += newMoveDirection;
                            }
                        }
                        Position -= moveDirection * (float)gameTime.ElapsedGameTime.TotalSeconds * movementSpeed;
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

            //// Get values from GlobalTransform for SpriteBatch and render sprite
            Vector2 position;
            Vector2 scale;
            float rotation;
            DecomposeMatrix(ref globalTransform, out position, out rotation, out scale);
            var imageCenter = new Vector2(Image.Width / 8, Image.Height / 8);
            spriteBatch.Draw(Image, position, null, Color.White, rotation - (float)(Math.PI * 0.5f), ImageCenter, scale, SpriteEffects.None, 0.0f);

            if (MainGame.IsDebugging)
            {
                spriteBatch.Draw(boundingBox, position, GetBoundingRectangle(), Color.White, rotation - (float)(Math.PI * 0.5f), ImageCenter, scale, SpriteEffects.None, 0.0f);
            }
        }

        private bool NewPositionBlocked(Vector2 newMovement)
        {
            Vector2 futurePosition = Position - Vector2.Normalize(newMovement) * Radius;
            Vector2 imageCenter = new Vector2(Parent.Image.Width / 2, Parent.Image.Height / 2);
            Vector2 playerCenterPosition = futurePosition + imageCenter;

            int x = (int)Math.Floor(playerCenterPosition.X / 35);
            int y = (int)Math.Floor(playerCenterPosition.Y / 35);

            if (((ShipCore)Parent).GetCollision(x, y))
            {
                return true;
            }

            return false;
        }
    }
}
