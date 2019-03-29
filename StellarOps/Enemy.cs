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

        private List<IEnumerator<int>> behaviours = new List<IEnumerator<int>>();

        public Enemy(ShipBase ship)
        {
            Ship = ship;
            AddBehavior(MoveForward());
        }

        public override void Update(GameTime gameTime, Matrix parentTransform)
        {
            ApplyBehaviours();
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
                Ship.MoveForward();
                yield return 0;
            }
        }
    }
}
