using Microsoft.Xna.Framework;

namespace StellarOps
{
    public class Tile
    {
        public Point Location { get; set; }

        public Rectangle Bounds { get; set; }

        public CollisionType CollisionType { get; set; }

        public TileType TileType { get; set; }

        public int? North { get; set; }

        public int? East { get; set; }

        public int? South { get; set; }

        public int? West { get; set; }

        public int Health { get; set; }
    }

    public enum CollisionType
    {
        Open,
        Collision
    }

    public enum TileType
    {
        Empty,
        Hull,
        Floor,
        Door,
        FlightConsole
    }
}
