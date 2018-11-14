using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace TestFlying
{
    public class Player
    {
        public Texture2D ship;
        public Vector2 position;
        Vector2 velocity;
        Vector2 acceleration;
        public float heading;
        const float thrust = 100.0f;

        public Player(Vector2 position)
        {
            this.position = position;
        }

        public void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            position += velocity * deltaTime;
            velocity += acceleration * deltaTime;
            acceleration.X = 0;
            acceleration.Y = 0;
        }

        public void HandleInput(KeyboardState KeyState)
        {
            if (KeyState.IsKeyDown(Keys.W) || KeyState.IsKeyDown(Keys.Up))
            {
                acceleration.X += thrust * (float)Math.Cos(heading);
                acceleration.Y += thrust * (float)Math.Sin(heading);
            }
            if (KeyState.IsKeyDown(Keys.A) || KeyState.IsKeyDown(Keys.Left))
            {
                heading -= 0.05F;
            }
            if (KeyState.IsKeyDown(Keys.D) || KeyState.IsKeyDown(Keys.Right))
            {
                heading += 0.05F;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            var shipCenter = new Vector2(ship.Width / 2, ship.Height / 2);
            spriteBatch.Draw(ship, position, null, Color.White, heading + (90 * (float)Math.PI / 180), shipCenter, 0.5f, SpriteEffects.None, 1f);
        }
    }
}
