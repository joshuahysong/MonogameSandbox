﻿using Microsoft.Xna.Framework;
using StellarOps.Contracts;

namespace StellarOps
{
    public class Bullet : Entity, IFocusable
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
            timeToLive = 10;
        }

        public override void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (Velocity.LengthSquared() > 0)
            {
                Orientation = Velocity.ToAngle();
            }

            Position += Velocity * deltaTime;

            // delete bullets that go off-screen
            timeAlive += gameTime.ElapsedGameTime.TotalSeconds;
            if (timeAlive > timeToLive)
            {
                IsExpired = true;
            }
        }
    }
}
