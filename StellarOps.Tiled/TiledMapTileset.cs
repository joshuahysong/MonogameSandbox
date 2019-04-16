using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace StellarOps.Tiled
{
    public class TiledMapTileset
    {
        public Texture2D Texture { get; set; }
        public int TileWidth { get; set; }
        public int TileHeight { get; set; }
        public int Spacing { get; set; }
        public int TileCount { get; set; }
        public int Columns { get; set; }
        public List<TiledMapTilesetTile> Tiles { get; set; }
    }
}
