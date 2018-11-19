using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace StellarOps
{
    public class Enemy : Entity
    {
        public static Random rand = new Random();

        private int timeUntilStart = 60;
        public bool IsActive { get { return timeUntilStart <= 0; } }
        private List<IEnumerator<int>> behaviours = new List<IEnumerator<int>>();

        public Enemy(Texture2D image, Vector2 position)
        {
            this.Image = image;
            Position = position;
            Radius = image.Width / 2f;
            color = Color.Transparent;
        }

        public override void Update(GameTime gametime)
        {
            if (timeUntilStart <= 0)
            {
                ApplyBehaviours();
            }
            else
            {
                timeUntilStart--;
                color = Color.White * (1 - timeUntilStart / 60f);
            }

            Position += Velocity;
            Position = Vector2.Clamp(Position, Size / 2, MainGame.ScreenSize - Size / 2);

            Velocity *= 0.8f;
        }

        public void WasShot()
        {
            IsExpired = true;
        }

        public static Enemy CreateSeeker(Vector2 position)
        {
            var enemy = new Enemy(Art.Seeker, position);
            enemy.AddBehaviour(enemy.FollowPlayer());

            return enemy;
        }

        public static Enemy CreateWanderer(Vector2 position)
        {
            var enemy = new Enemy(Art.Wanderer, position);
            enemy.AddBehaviour(enemy.MoveRandomly());
            return enemy;
        }

        public void HandleCollision(Enemy other)
        {
            var d = Position - other.Position;
            Velocity += 10 * d / (d.LengthSquared() + 1);
        }

        private void AddBehaviour(IEnumerable<int> behaviour)
        {
            behaviours.Add(behaviour.GetEnumerator());
        }

        private void ApplyBehaviours()
        {
            for (int i = 0; i < behaviours.Count; i++)
            {
                if (!behaviours[i].MoveNext())
                {
                    behaviours.RemoveAt(i--);
                }
            }
        }

        IEnumerable<int> FollowPlayer(float acceleration = 1f)
        {
            while (true)
            {
                Velocity += (MainGame.Player.Position - Position).ScaleTo(acceleration);
                if (Velocity != Vector2.Zero)
                {
                    Heading = Velocity.ToAngle();
                }

                yield return 0;
            }
        }

        IEnumerable<int> MoveRandomly()
        {
            float direction = rand.NextFloat(0, MathHelper.TwoPi);

            while (true)
            {
                direction += rand.NextFloat(-0.1f, 0.1f);
                direction = MathHelper.WrapAngle(direction);

                for (int i = 0; i < 6; i++)
                {
                    Velocity += MathUtil.FromPolar(direction, 0.4f);
                    Heading -= 0.05f;

                    var bounds = MainGame.Viewport.Bounds;
                    bounds.Inflate(-Image.Width, -Image.Height);

                    // if the enemy is outside the bounds, make it move away from the edge
                    if (!bounds.Contains(Position.ToPoint()))
                        direction = (MainGame.ScreenSize / 2 - Position).ToAngle() + rand.NextFloat(-MathHelper.PiOver2, MathHelper.PiOver2);

                    yield return 0;
                }
            }
        }
    }
}
