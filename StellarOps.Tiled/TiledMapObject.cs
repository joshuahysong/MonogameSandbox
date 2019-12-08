using System.Collections.Generic;

namespace StellarOps.Tiled
{
    public class TiledMapObject
    {
        public int Id { get; set; }
        public float Rotation { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public Dictionary<string,string> Properties { get; set; }

        TiledMapObject()
        {
            Properties = new Dictionary<string, string>();
        }
    }
}
