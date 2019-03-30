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
            AddBehavior(HuntPlayer());
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

        private IEnumerable<int> HuntPlayer()
        {
            while (true)
            {
                float distanceToTarget = (Position - Target.Position).Length();
                double degreesToTarget = GetDegreesToTarget();
                if (distanceToTarget > 1000)
                {
                    Ship.ApplyForwardThrust();
                }
                if (degreesToTarget > 175 && degreesToTarget < 185)
                {
                    Ship.FireWeapons();
                }
                if (degreesToTarget <= 179)
                {
                    Ship.ApplyStarboardManeuveringThrusters();
                }
                else if (degreesToTarget > 181)
                {
                    Ship.ApplyPortManeuveringThrusters();
                }
                else
                {
                    Ship.AreManeuveringThrustersFiring = false;
                }
                yield return 0;
            }
        }

        private double GetDegreesToTarget()
        {
            float angleToTarget = (Position - Target.Position).ToAngle();
            double degreesToTarget = angleToTarget.ToDegrees();
            double headingDegrees = Ship.Heading.ToDegrees();
            return headingDegrees < degreesToTarget ? (headingDegrees + 360) - degreesToTarget : headingDegrees - degreesToTarget;
        }
    }
}
