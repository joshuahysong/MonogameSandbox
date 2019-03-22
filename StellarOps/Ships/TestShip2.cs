using Microsoft.Xna.Framework;
namespace StellarOps.Ships
{
    public class TestShip2 : ShipCore
    {
        public TestShip2(Vector2 spawnPosition)
        {
            Position = Vector2.Zero;
            Heading = 0.0f;
            Size = new Vector2(17 * MainGame.TileSize, 10 * MainGame.TileSize);
            Thrust = 250f;
            MaxTurnRate = 0.65f;
            ManeuveringThrust = 0.01f;
            MaxVelocity = 500f;
            TileMapData = new int[,]
            {
                {0,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0},
                {0,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0},
                {0,1,1,1,1,1,1,2,2,1,1,1,0,0,0,0,0},
                {1,1,2,2,2,2,1,2,2,2,2,1,1,1,1,1,1},
                {1,2,2,2,1,2,2,2,2,2,2,2,2,2,2,4,1},
                {1,2,2,2,1,2,2,2,2,2,2,2,2,2,2,2,1},
                {1,1,2,2,2,2,1,2,2,2,2,1,1,1,1,1,1},
                {0,1,1,1,1,1,1,2,2,1,1,1,0,0,0,0,0},
                {0,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0},
                {0,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0}
            };
            TileMap = GetTileMap();
        }

        public override void UseTile(Vector2 position)
        {
            if (GetTile(position).TileType == TileType.FlightConsole)
            {
                SwitchControlToShip();
            }
        }

        public override string GetUsePrompt(Vector2 position)
        {
            if (GetTile(position).TileType == TileType.FlightConsole)
            {
                return "Use Flight Control";
            }
            return string.Empty;
        }
    }
}
