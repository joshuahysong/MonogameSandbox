using Microsoft.Xna.Framework.Content.Pipeline;
using Newtonsoft.Json;
using StellarOps.Tiled;
using System.IO;

namespace StellarOps.Content.Pipeline
{
    [ContentImporter(".json", DefaultProcessor = "TiledMapProcessor",
    DisplayName = "Tiled Map Importer")]
    public class TiledMapImporter : ContentImporter<TiledMap>
    {
        public override TiledMap Import(string filename, ContentImporterContext context)
        {
            context.Logger.LogMessage("Importing JSON map: {0}", filename);
            using (var file = File.OpenText(filename))
            {
                JsonSerializer serializer = new JsonSerializer();
                TiledMap serializedMap = (TiledMap)serializer.Deserialize(file, typeof(TiledMap));
                return serializedMap;
            }
        }
    }
}
