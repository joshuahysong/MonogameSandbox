using System.Collections.Generic;

namespace StellarOps.Tiled
{
    public class TiledMapTileLayer
    {
        public string Name { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public TiledMapTile[] Tiles { get; set; }
    }
}
