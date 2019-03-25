using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StellarOps.Projectiles
{
    public class ProjectileBase : Entity
    {
        public Texture2D Image { get; set; }
        public float Radius { get; set; }

        public Vector2 Size =>Image == null ? Vector2.Zero : new Vector2(Image.Width, Image.Height);

        private long _timeToLive;
        private double _timeAlive;

        public ProjectileBase(Vector2 position, Vector2 velocity)
        {
            Image = Art.Bullet;
            Position = position;
            Velocity = velocity;
            Heading = Velocity.ToAngle();
            Radius = 8;
            _timeToLive = 100;
        }

        public override void Update(GameTime gameTime, Matrix parentTransform)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (Velocity.LengthSquared() > 0)
            {
                Heading = Velocity.ToAngle();
            }

            Position += Velocity * deltaTime;

            // delete bullets that go off-screen
            _timeAlive += gameTime.ElapsedGameTime.TotalSeconds;
            if (_timeAlive > _timeToLive)
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
