using Microsoft.Xna.Framework;

namespace StellarOps.Projectiles
{
    public class TestProjectile : ProjectileBase
    {
        public TestProjectile(Vector2 position, Vector2 velocity) : base(position, velocity)
        {
            Image = Art.Bullet;
            Radius = 8;
            TimeToLive = 100;
        }
    }
}
