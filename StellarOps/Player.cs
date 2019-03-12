using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StellarOps.Contracts;
using System;

namespace StellarOps
{
    public class Player : Entity, IFocusable
    {
        public Vector2 WorldPosition { get; set; }

        public Player()
        {
            Image = Art.Player;
            Position = Vector2.Zero;
            IsChild = true;
        }

        public override void Update(GameTime gameTime, Matrix parentTransform)
        {
            HandleInput(gameTime, parentTransform);
            HandleCollisions();
            GetTilePosition();
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
                if (Input.MouseState.LeftButton == ButtonState.Pressed)
                {
                    Vector2 targetPosition = Vector2.Transform(Input.MouseState.Position.ToVector2(), Matrix.Invert(MainGame.Camera.Transform));

                    if (!float.IsNaN(targetPosition.X) && !float.IsNaN(targetPosition.X))
                    {
                        Vector2 direction = Vector2.Normalize(targetPosition - position);

                        // Get parent rotation
                        Vector2 parentPosition;
                        Vector2 parentScale;
                        float parentRotation;
                        DecomposeMatrix(ref parentTransform, out parentPosition, out parentRotation, out parentScale);
                        Heading = (float)Math.Atan2(direction.Y, direction.X) - parentRotation;
                        Vector2 moveDirection = Vector2.Normalize(new Vector2((float)Math.Cos(Heading), (float)Math.Sin(Heading)));
                        Position += (moveDirection * (float)gameTime.ElapsedGameTime.TotalSeconds * 50.0f);
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
                spriteBatch.Draw(boundingBox, position, BoundingRectangle, Color.White, rotation - (float)(Math.PI * 0.5f), ImageCenter, scale, SpriteEffects.None, 0.0f);
            }
        }

        /// <summary>
        /// Detects and resolves all collisions between the player and his neighboring
        /// tiles. When a collision is detected, the player is pushed away along one
        /// axis to prevent overlapping. There is some special logic for the Y axis to
        /// handle platforms which behave differently depending on direction of movement.
        /// </summary>
        private void HandleCollisions()
        {
            // Get the player's bounding rectangle and find neighboring tiles.
            //Rectangle bounds = BoundingRectangle;
            //int leftTile = (int)Math.Floor((float)bounds.Left / 35);
            //int rightTile = (int)Math.Ceiling((float)bounds.Right / 35) - 1;
            //int topTile = (int)Math.Floor((float)bounds.Top / 35);
            //int bottomTile = (int)Math.Ceiling((float)bounds.Bottom / 35) - 1;

            //// For each potentially colliding tile,
            //for (int y = topTile; y <= bottomTile; ++y)
            //{
            //    for (int x = leftTile; x <= rightTile; ++x)
            //    {
            //        // If this tile is collidable,
            //        var collision = ((ShipCore)Parent).GetCollision(x,y);
            //        if (collision)
            //        {
            //            // Determine collision depth (with direction) and magnitude.
            //            Rectangle tileBounds = ((ShipCore)Parent).GetBounds(x, y);
            //            Vector2 depth = bounds.GetIntersectionDepth(tileBounds);
            //            if (depth != Vector2.Zero)
            //            {
            //                float absDepthX = Math.Abs(depth.X);
            //                float absDepthY = Math.Abs(depth.Y);
            //                Console.WriteLine("COLLISION");
            //                //// Resolve the collision along the shallow axis.
            //                //if (absDepthY < absDepthX || collision == TileCollision.Platform)
            //                //{
            //                //    // If we crossed the top of a tile, we are on the ground.
            //                //    if (previousBottom <= tileBounds.Top)
            //                //        isOnGround = true;

            //                //    // Ignore platforms, unless we are on the ground.
            //                //    if (collision == TileCollision.Impassable || IsOnGround)
            //                //    {
            //                //        // Resolve the collision along the Y axis.
            //                //        Position = new Vector2(Position.X, Position.Y + depth.Y);

            //                //        // Perform further collisions with the new bounds.
            //                //        bounds = BoundingRectangle;
            //                //    }
            //                //}
            //                //else if (collision == TileCollision.Impassable) // Ignore platforms.
            //                //{
            //                //    // Resolve the collision along the X axis.
            //                //    Position = new Vector2(Position.X + depth.X, Position.Y);

            //                //    // Perform further collisions with the new bounds.
            //                //    bounds = BoundingRectangle;
            //                //}
            //            }
            //        }
            //    }
            //}

            // Save the new bounds bottom.
            //previousBottom = bounds.Bottom;
        }
    }
}
