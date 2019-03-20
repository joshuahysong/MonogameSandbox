using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace StellarOps.Contracts
{
    public interface IContainer
    {
        Vector2 Position { get; set; }

        Vector2 ImageCenter { get; }

        List<Tile> TileMap { get; set; }

        List<IPawn> Pawns { get; set; }

        Tile GetTile(Vector2 position);

        void UseTile(Vector2 position);

        string GetUsePrompt(Vector2 position);
    }
}
