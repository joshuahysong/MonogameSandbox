using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StellarOps.Ships;
using System;
using System.Collections.Generic;

namespace StellarOps
{
    public class Enemy : Entity
    {
        public ShipBase Ship { get; set; }
        public Entity Target { get; set; }

        private List<IEnumerator<int>> behaviours = new List<IEnumerator<int>>();

        public Enemy(ShipBase ship)
        {
            Ship = ship;
            Target = MainGame.Ship;
            AddBehavior(MoveForward());
        }

        public override void Update(GameTime gameTime, Matrix parentTransform)
        {
            ApplyBehaviours();
            Position = Ship.Position;
            Ship.Update(gameTime, parentTransform);
        }

        public override void Draw(SpriteBatch spriteBatch, Matrix parentTransform)
        {
            Ship.Draw(spriteBatch, parentTransform);
        }

        private void AddBehavior(IEnumerable<int> behaviour)
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

        private IEnumerable<int> MoveForward()
        {
            while (true)
            {
                for (int i = 0; i < 80; i++)
                {
                    Ship.ApplyStarboardManeuveringThrusters();
                    if (IsTargetInRange())
                    {
                        Ship.FireWeapons();
                    }
                    yield return 0;
                }
                for (int i = 0; i < 40; i++)
                {
                    Ship.AreManeuveringThrustersFiring = false;
                    Ship.ApplyForwardThrust();
                    if (IsTargetInRange())
                    {
                        Ship.FireWeapons();
                    }
                    yield return 0;
                }
            }
        }

        private bool IsTargetInRange()
        {
            float distanceToTarget = (Position - Target.Position).Length();
            if (distanceToTarget > 5000)
            {
                return false;
            }

            float angleToTarget = (Position - Target.Position).ToAngle();
            double degreesToTarget = angleToTarget.ToDegrees();
            double headingDegrees = Ship.Heading.ToDegrees();
            double targetDifference = headingDegrees < degreesToTarget ? (headingDegrees + 360) - degreesToTarget : headingDegrees - degreesToTarget;

            return targetDifference <= 200 && targetDifference >= 160;
        }
    }
}
