using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StellarOps.Projectiles;
using System;

namespace StellarOps.Weapons
{
    public class WeaponBase : Entity
    {
        public Texture2D Image { get; set; }

        protected float Cooldown;
        protected float Speed;
        protected float Accuracy;

        private float _cooldownRemaining;
        private static Random _random = new Random();

        public Vector2 Center => Image == null ? Vector2.Zero : new Vector2(Image.Width / 2, Image.Height / 2);

        public WeaponBase()
        {
            Cooldown = 25f;
            Speed = 1000f;
            Accuracy = 99f;
            Position = new Vector2(285, 0);
        }

        public override void Update(GameTime gameTime, Matrix parentTransform) { }

        public override void Draw(SpriteBatch spriteBatch, Matrix parentTransform)
        {
            // Calculate global transform
            Matrix globalTransform = LocalTransform * parentTransform;

            // Get values from GlobalTransform for SpriteBatch and render sprite
            DecomposeMatrix(ref globalTransform, out Vector2 position, out float rotation, out Vector2 scale);
            spriteBatch.Draw(Image, position, null, Color.White, rotation - (float)(Math.PI * 0.5f), Center, scale * MainGame.PawnScale, SpriteEffects.None, 0.0f);
        }

        public void Fire(float heading, Vector2 relativeVelocity, Vector2 shipLocation)
        {
            if (_cooldownRemaining <= 0)
            {
                _cooldownRemaining = Cooldown;
                Quaternion aimQuat = Quaternion.CreateFromYawPitchRoll(0, 0, heading);

                float jitter = 1 - Accuracy / 100;
                float randomSpread = _random.NextFloat(-jitter, jitter) + _random.NextFloat(-jitter, jitter);
                Vector2 vel = MathUtil.FromPolar(heading + randomSpread, Speed);

                Vector2 offset = Vector2.Transform(Position, aimQuat);
                EntityManager.Add(new ProjectileBase(shipLocation + offset, vel + relativeVelocity));
            }

            if (_cooldownRemaining > 0)
            {
                _cooldownRemaining--;
            }
        }
    }
}
