using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StellarOps.Ships
{
    public class TestShip : ShipCore
    {
        public TestShip(Vector2 spawnPosition) : base()
        {
            Position = Vector2.Zero;
            Heading = 0.0f;
            Thrust = 250;
            TurnRate = 0.02f;
            MaxVelocity = 400f;
            Image = Art.TestShip;
            InteriorImage = Art.TestShipInterior;
        }
    }
}
