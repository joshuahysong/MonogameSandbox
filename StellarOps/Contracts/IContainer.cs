using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace StellarOps.Contracts
{
    public interface IContainer
    {
        Vector2 Position { get; }

        Vector2 Center { get; }

        Vector2 Size { get; }

        List<Tile> TileMap { get; }

        List<IPawn> Pawns { get; set; }

        Tile GetTile(Vector2 position);

        Tile GetTile(Point location);

        void UseTile(Vector2 position);

        string GetUsePrompt(Vector2 position);
    }
}
