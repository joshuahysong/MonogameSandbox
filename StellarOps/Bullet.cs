using Microsoft.Xna.Framework;

namespace StellarOps
{
    public class Bullet : Entity
    {
        long timeToLive;
        double timeAlive;

        public Bullet(Vector2 position, Vector2 velocity)
        {
            image = Art.Bullet;
            Position = position;
            Velocity = velocity;
            Orientation = Velocity.ToAngle();
            Radius = 8;
            timeToLive = 2;
        }

        public override void Update(GameTime gameTime)
        {
            if (Velocity.LengthSquared() > 0)
            {
                Orientation = Velocity.ToAngle();
            }

            Position += Velocity;

            // delete bullets that go off-screen
            timeAlive += gameTime.ElapsedGameTime.TotalSeconds;
            if (timeAlive > timeToLive)
            {
                IsExpired = true;
            }
        }
    }
}
