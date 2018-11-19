using Microsoft.Xna.Framework;
using System;

namespace StellarOps
{
    public class Weapon
    {
        public float Cooldown;
        public Vector2 AttachedPosition;
        public float CooldownRemaining;
        public float Speed;
        public float Accuracy;

        static Random rand = new Random();

        public void Fire(float heading, Vector2 relativeVelocity, Vector2 shipLocation)
        {
            if (CooldownRemaining <= 0)
            {
                CooldownRemaining = Cooldown;
                Quaternion aimQuat = Quaternion.CreateFromYawPitchRoll(0, 0, heading);

                float jitter = 1 - Accuracy / 100;
                float randomSpread = rand.NextFloat(-jitter, jitter) + rand.NextFloat(-jitter, jitter);
                Vector2 vel = MathUtil.FromPolar(heading + randomSpread, Speed);

                Vector2 offset = Vector2.Transform(AttachedPosition, aimQuat);
                EntityManager.Add(new Bullet(shipLocation + offset, vel + relativeVelocity));
            }

            if (CooldownRemaining > 0)
            {
                CooldownRemaining--;
            }
        }
    }
}
