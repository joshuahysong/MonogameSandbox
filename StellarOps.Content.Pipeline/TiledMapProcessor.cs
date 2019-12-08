using Microsoft.Xna.Framework.Content.Pipeline;
using StellarOps.Tiled;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StellarOps.Content.Pipeline
{
    [ContentProcessor(DisplayName = "Tiled Map Processor")]
    public class TiledMapProcessor : ContentProcessor<TiledMap, TiledMap>
    {
        public override TiledMap Process(TiledMap input, ContentProcessorContext context)
        {
            return input;
        }
    }
}
