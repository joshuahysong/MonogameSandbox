using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StellarOps.Projectiles
{
    public abstract class ProjectileBase : Entity
    {
        protected Texture2D Image;
        protected float Radius;
        protected long TimeToLive;

        protected Vector2 Size =>Image == null ? Vector2.Zero : new Vector2(Image.Width, Image.Height);

        private double _timeAlive;

        public ProjectileBase(Vector2 position, Vector2 velocity)
        {
            Image = Art.Bullet;
            Position = position;
            Velocity = velocity;
            Heading = Velocity.ToAngle();
        }

        public override void Update(GameTime gameTime, Matrix parentTransform)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (Velocity.LengthSquared() > 0)
            {
                Heading = Velocity.ToAngle();
            }

            Position += Velocity * deltaTime;

            _timeAlive += gameTime.ElapsedGameTime.TotalSeconds;
            if (_timeAlive > TimeToLive)
            {
                IsExpired = true;
            }
        }

        public override void Draw(SpriteBatch spriteBatch, Matrix parentTransform)
        {
            // TODO Only draw if on screen
            spriteBatch.Draw(Image, Position, null, color, Heading, Size / 2f, 1f, 0, 0);
        }
    }
}
