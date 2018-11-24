﻿using Microsoft.Xna.Framework;
using System;

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
            tileMap = new int[,] 
            {
                {0,0,0,0,0,0,1,1,0,0,0,0,0,0,0},
                {0,1,1,1,1,0,1,1,1,1,0,0,0,0,0},
                {1,1,1,0,1,1,1,1,1,1,1,1,1,1,1},
                {1,1,1,0,1,1,1,1,1,1,1,1,1,1,1},
                {0,1,1,1,1,0,1,1,1,1,0,0,0,0,0},
                {0,0,0,0,0,0,1,1,0,0,0,0,0,0,0}
            };
        }
    }
}
