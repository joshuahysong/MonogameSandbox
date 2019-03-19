using Microsoft.Xna.Framework;

namespace StellarOps.Ships
{
    public class TestShip : ShipCore
    {
        public TestShip(Vector2 spawnPosition) : base()
        {
            Position = Vector2.Zero;
            Heading = 0.0f;
            Thrust = 250f;
            MaxTurnRate = 0.65f;
            ManeuveringThrust = 0.01f;
            MaxVelocity = 500f;
            Image = Art.TestShip;
            InteriorImage = Art.TestShipInterior;
            TileMap = new int[,]
            {
                {0,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0},
                {0,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0},
                {0,1,1,1,1,1,1,2,2,1,1,1,0,0,0,0,0},
                {1,1,2,2,2,2,1,2,2,2,2,1,1,1,1,1,1},
                {1,2,2,2,1,2,2,2,2,2,2,2,2,2,2,3,1},
                {1,2,2,2,1,2,2,2,2,2,2,2,2,2,2,2,1},
                {1,1,2,2,2,2,1,2,2,2,2,1,1,1,1,1,1},
                {0,1,1,1,1,1,1,2,2,1,1,1,0,0,0,0,0},
                {0,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0},
                {0,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0}
            };
        }

        public override void PerformUseAtTile(Vector2 position)
        {
            if (GetTileAtPosition(position) == new Vector2(15,4))
            {
                SwitchControlToShip();
            }
        }

        public override string GetTileText(Vector2 position)
        {
            if (GetTileAtPosition(position) == new Vector2(15, 4))
            {
                return "Use Flight Control";
            }
            return string.Empty;
        }
    }
}
