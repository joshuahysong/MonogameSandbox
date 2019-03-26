﻿using Microsoft.Xna.Framework;
using StellarOps.Weapons;

namespace StellarOps.Ships
{
    public class TestShip : ShipBase
    {
        public TestShip(Vector2 spawnPosition)
        {
            Position = Vector2.Zero;
            Heading = 0.0f;
            Size = new Vector2(17 * MainGame.TileSize, 10 * MainGame.TileSize);
            Thrust = 250f;
            MaxTurnRate = 0.65f;
            ManeuveringThrust = 0.01f;
            MaxVelocity = 500f;
            Weapons.Add(new TestWeapon(new Vector2(13 * MainGame.TileSize, 1 * MainGame.TileSize) - Center));
            Weapons.Add(new TestWeapon(new Vector2(13 * MainGame.TileSize, 8 * MainGame.TileSize) - Center));
            TileMapArtData = new int[,]
            {
                {0,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0},
                {0,5,5,5,5,5,5,1,1,1,0,6,0,0,0,0,0},
                {0,1,1,1,1,1,1,2,2,1,1,1,0,0,0,0,0},
                {1,1,2,2,2,2,1,2,2,2,2,1,1,1,1,1,1},
                {1,2,2,2,1,2,2,2,2,2,2,2,2,2,2,4,1},
                {1,2,2,2,1,2,2,2,2,2,2,2,2,2,2,2,1},
                {1,1,2,2,2,2,1,2,2,2,2,1,1,1,1,1,1},
                {0,1,1,1,1,1,1,2,2,1,1,1,0,0,0,0,0},
                {0,5,5,5,5,5,5,1,1,1,0,6,0,0,0,0,0},
                {0,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0}
            };
            TileMapCollisionData = new int[,]
            {
                {0,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0},
                {0,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0},
                {0,1,1,1,1,1,1,0,0,1,1,1,0,0,0,0,0},
                {1,1,0,0,0,0,1,0,0,0,0,1,1,1,1,1,1},
                {1,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,1},
                {1,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,1},
                {1,1,0,0,0,0,1,0,0,0,0,1,1,1,1,1,1},
                {0,1,1,1,1,1,1,0,0,1,1,1,0,0,0,0,0},
                {0,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0},
                {0,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0}
            };
            TileMapHealthData = new int[,]
            {
                {0,100,100,100,100,100,100,100,0,0,0,0,0,0,0,0,0},
                {0,100,100,100,100,100,100,100,100,100,0,100,100,0,0,0,0},
                {0,100,100,100,100,100,100,100,100,100,100,100,0,0,0,0,0},
                {100,100,100,100,100,100,100,100,100,100,100,100,100,100,100,100,100},
                {100,100,100,100,100,100,100,100,100,100,100,100,100,100,100,100,100},
                {100,100,100,100,100,100,100,100,100,100,100,100,100,100,100,100,100},
                {100,100,100,100,100,100,100,100,100,100,100,100,100,100,100,100,100},
                {0,100,100,100,100,100,100,100,100,100,100,100,0,0,0,0,0},
                {0,100,100,100,100,100,100,100,100,100,0,100,100,0,0,0,0},
                {0,100,100,100,100,100,100,100,0,0,0,0,0,0,0,0,0}
            };
            TileMap = GetTileMap();
        }

        public override void UseTile(Vector2 position)
        {
            Maybe<Tile> tile = GetTileByRelativePosition(position);
            if (tile.HasValue && tile.Value.TileType == TileType.FlightConsole)
            {
                SwitchControlToShip();
            }
        }

        public override string GetUsePrompt(Vector2 position)
        {
            Maybe<Tile> tile = GetTileByRelativePosition(position);
            if (tile.HasValue && tile.Value.TileType == TileType.FlightConsole)
            {
                return "Use Flight Control";
            }
            return string.Empty;
        }
    }
}
