using Microsoft.Xna.Framework;
using StellarOps.Weapons;

namespace StellarOps.Ships
{
    public class TestShip : ShipBase
    {
        public TestShip(Vector2 spawnPosition, float spawnHeading)
        {
            Position = spawnPosition;
            Heading = spawnHeading;
            Size = new Vector2(17 * MainGame.TileSize, 12 * MainGame.TileSize);
            Thrust = 250f;
            MaxTurnRate = 0.65f;
            ManeuveringThrust = 0.01f;
            MaxVelocity = 500f;
            Weapons.Add(new TestWeapon(new Vector2(13 * MainGame.TileSize, 2 * MainGame.TileSize) - Center));
            Weapons.Add(new TestWeapon(new Vector2(13 * MainGame.TileSize, 9 * MainGame.TileSize) - Center));
            TileMapArtData = new int[,]
            {
                {0,0,8,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0},
                {7,5,5,5,5,5,5,1,1,1,0,6,0,0,0,0,0},
                {0,1,1,1,1,1,1,2,2,1,1,1,0,0,9,0,0},
                {1,1,2,2,2,2,1,2,2,2,2,1,1,1,1,1,1},
                {1,2,2,2,1,2,2,2,2,2,2,2,2,2,2,4,1},
                {1,2,2,2,1,2,2,2,2,2,2,2,2,2,2,2,1},
                {1,1,2,2,2,2,1,2,2,2,2,1,1,1,1,1,1},
                {0,1,1,1,1,1,1,2,2,1,1,1,0,0,8,0,0},
                {7,5,5,5,5,5,5,1,1,1,0,6,0,0,0,0,0},
                {0,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0},
                {0,0,9,0,0,0,0,0,0,0,0,0,0,0,0,0,0}
            };
            TileMapCollisionData = new int[,]
            {
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0},
                {0,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0},
                {0,1,1,1,1,1,1,0,0,1,1,1,0,0,0,0,0},
                {1,1,0,0,0,0,1,0,0,0,0,1,1,1,1,1,1},
                {1,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,1},
                {1,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,1},
                {1,1,0,0,0,0,1,0,0,0,0,1,1,1,1,1,1},
                {0,1,1,1,1,1,1,0,0,1,1,1,0,0,0,0,0},
                {0,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0},
                {0,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}
            };
            TileMapHealthData = new int[,]
            {
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,100,100,100,100,100,100,100,0,0,0,0,0,0,0,0,0},
                {0,100,100,100,100,100,100,100,100,100,0,100,100,0,0,0,0},
                {0,100,100,100,100,100,100,100,100,100,100,100,0,0,0,0,0},
                {100,100,100,100,100,100,100,100,100,100,100,100,100,100,100,100,100},
                {100,100,100,100,100,100,100,100,100,100,100,100,100,100,100,100,100},
                {100,100,100,100,100,100,100,100,100,100,100,100,100,100,100,100,100},
                {100,100,100,100,100,100,100,100,100,100,100,100,100,100,100,100,100},
                {0,100,100,100,100,100,100,100,100,100,100,100,0,0,0,0,0},
                {0,100,100,100,100,100,100,100,100,100,0,100,100,0,0,0,0},
                {0,100,100,100,100,100,100,100,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}
            };
            Tiles = GetTiles();
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
