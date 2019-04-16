using System.Collections.Generic;

namespace StellarOps.Tiled
{
    public class TiledMapTilesetTile
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public Dictionary<string, string> Properties { get; set; }
    }
}
