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
        List<Tile> Tiles { get; }
        List<IPawn> Pawns { get; set; }
        bool IsManeuvering { get; set; }
        bool IsMainThrustFiring { get; set; }
        bool IsPortThrustFiring { get; set; }
        bool IsStarboardThrustFiring { get; set; }

        Maybe<Tile> GetTileByWorldPosition(Vector2 position);
        Maybe<Tile> GetTileByRelativePosition(Vector2 position);
        Maybe<Tile> GetTileByPoint(Point location);
        void UseTile(Vector2 position);
        string GetUsePrompt(Vector2 position);
    }
}
