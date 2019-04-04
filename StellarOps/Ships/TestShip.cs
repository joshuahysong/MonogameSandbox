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
            SetTilesFromTiledMap(Art.TestShip);
        }

        public override void UseTile(Vector2 position)
        {
            Maybe<Tile> tile = GetTileByRelativePosition(position);
            if (tile.HasValue && tile.Value.TileType == TileType.FlightControl)
            {
                SwitchControlToShip();
            }
        }

        public override string GetUsePrompt(Vector2 position)
        {
            Maybe<Tile> tile = GetTileByRelativePosition(position);
            if (tile.HasValue && tile.Value.TileType == TileType.FlightControl)
            {
                return "Use Flight Control";
            }
            return string.Empty;
        }
    }
}
