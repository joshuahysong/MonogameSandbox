using Microsoft.Xna.Framework;

namespace StellarOps
{
    public struct Tile
    {
        public Point Location { get; set; }

        public Rectangle Bounds { get; set; }

        public bool Collidable { get; set; }

        public TileType TileType { get; set; }
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
