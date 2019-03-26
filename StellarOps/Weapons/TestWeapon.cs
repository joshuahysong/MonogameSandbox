using Microsoft.Xna.Framework;
using StellarOps.Projectiles;

namespace StellarOps.Weapons
{
    public class TestWeapon : WeaponBase
    {
        public TestWeapon(Vector2 position) : base(position)
        {
            Cooldown = 25f;
            Speed = 2000f;
            Accuracy = 99f;
            projectileType = typeof(TestProjectile);
        }
    }
}
