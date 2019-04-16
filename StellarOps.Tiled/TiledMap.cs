using System.Collections.Generic;

namespace StellarOps.Tiled
{
    public class TiledMap
    {
        public int Height { get; set; }
        public int Width { get; set; }
        public int TileHeight { get; set; }
        public int TileWidth { get; set; }
        public List<TiledMapTileLayer> TileLayers { get; set; }
        public List<TiledMapObjectLayer> ObjectLayers { get; }
        public List<TiledMapTileset> TileSets { get; set; }
    }
}
