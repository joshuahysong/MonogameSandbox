using Microsoft.Xna.Framework;
using System;

namespace StellarOps.Projectiles
{
    public class TestProjectile : ProjectileBase
    {
        public TestProjectile(Vector2 position, Vector2 velocity) : base(position, velocity)
        {
            Image = Art.Bullet;
            Radius = (float)Math.Ceiling((double)(Image.Width / 2));
            TimeToLive = 20;
        }
    }
}
