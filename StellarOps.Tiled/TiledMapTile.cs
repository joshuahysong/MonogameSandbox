namespace StellarOps.Tiled
{
    public class TiledMapTile
    {
        public int X;
        public int Y;
        public uint IdWithFlags;
        public int Id => (int)(IdWithFlags & ~(uint)TiledMapTileFlipFlags.All);
        public bool IsFlippedHorizontally => (IdWithFlags & (uint)TiledMapTileFlipFlags.FlipHorizontally) != 0;
        public bool IsFlippedVertically => (IdWithFlags & (uint)TiledMapTileFlipFlags.FlipVertically) != 0;
        public bool IsFlippedDiagonally => (IdWithFlags & (uint)TiledMapTileFlipFlags.FlipDiagonally) != 0;
        public bool IsBlank => Id == 0;
        public TiledMapTile(uint idWithFlags, int x, int y)
        {
            X = x;
            Y = y;
            IdWithFlags = idWithFlags;
        }
    }
}
