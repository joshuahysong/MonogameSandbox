using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StellarOps.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StellarOps
{
    public static class TileHelper
    {
        private static List<TileType> EmptyTileTypes = new List<TileType>
        {
            TileType.Empty,
            TileType.MainThrust,
            TileType.PortThrust,
            TileType.StarboardThrust,
            TileType.Weapon
        };

        public static Tuple<Texture2D, Rectangle, float> GetTileData(Tile tile, IContainer container)
        {
            if (tile.TileType == TileType.Weapon)
            {
                return new Tuple<Texture2D, Rectangle, float>(Art.Weapon, Art.Weapon.Bounds, 0);
            }
            if (tile.TileType == TileType.Engine)
            {
                return new Tuple<Texture2D, Rectangle, float>(Art.Engine, Art.Engine.Bounds, 0);
            }
            if (tile.TileType == TileType.Floor)
            {
                return new Tuple<Texture2D, Rectangle, float>(Art.Floor, Art.Floor.Bounds, 0);
            }
            if (tile.TileType == TileType.FlightConsole)
            {
                return new Tuple<Texture2D, Rectangle, float>(Art.FlightConsole, Art.FlightConsole.Bounds, 0);
            }
            if (tile.TileType == TileType.MainThrust || tile.TileType == TileType.PortThrust || tile.TileType == TileType.StarboardThrust)
            {
                return new Tuple<Texture2D, Rectangle, float>(Art.MainThruster, Art.MainThruster.Bounds, 0);
            }
            if (tile.TileType == TileType.Hull)
            {
                TileType north = tile.North == null ? TileType.Empty : container.Tiles[(int)tile.North].TileType;
                TileType east = tile.East == null ? TileType.Empty : container.Tiles[(int)tile.East].TileType;
                TileType south = tile.South == null ? TileType.Empty : container.Tiles[(int)tile.South].TileType;
                TileType west = tile.West == null ? TileType.Empty : container.Tiles[(int)tile.West].TileType;

                // Hull Full
                if (north == TileType.Hull && east == TileType.Hull && south == TileType.Hull && west == TileType.Hull)
                {
                }
                // Hull Single
                if (north != TileType.Hull && east != TileType.Hull && south != TileType.Hull && west != TileType.Hull)
                {
                }
                // Hull Horizontal North
                if (EmptyTileTypes.Contains(north) && east == TileType.Hull && south != TileType.Hull && !EmptyTileTypes.Contains(south) && west == TileType.Hull)
                {
                    return new Tuple<Texture2D, Rectangle, float>(Art.Hull, new Rectangle(0, 0, Art.TileSize, Art.TileSize), 0);
                }
                // Hull Horizontal South
                if (north != TileType.Hull && !EmptyTileTypes.Contains(north) && east == TileType.Hull && EmptyTileTypes.Contains(south) && west == TileType.Hull)
                {
                    return new Tuple<Texture2D, Rectangle, float>(Art.Hull, new Rectangle(0, 0, Art.TileSize, Art.TileSize), (float)Math.PI);
                }
                // Hull Vertical West
                if (north == TileType.Hull && east != TileType.Hull && !EmptyTileTypes.Contains(east) && south == TileType.Hull && EmptyTileTypes.Contains(west))
                {
                    return new Tuple<Texture2D, Rectangle, float>(Art.Hull, new Rectangle(0, 0, Art.TileSize, Art.TileSize), -(float)Math.PI / 2);
                }
                // Hull Vertical East
                if (north == TileType.Hull && EmptyTileTypes.Contains(east) && south == TileType.Hull && west != TileType.Hull && !EmptyTileTypes.Contains(west))
                {
                    return new Tuple<Texture2D, Rectangle, float>(Art.Hull, new Rectangle(0, 0, Art.TileSize, Art.TileSize), (float)Math.PI / 2);
                }
                // Hull Inner Corner NE
                if (north == TileType.Hull && east == TileType.Hull && south != TileType.Hull && !EmptyTileTypes.Contains(south) && west != TileType.Hull && !EmptyTileTypes.Contains(west))
                {
                    return new Tuple<Texture2D, Rectangle, float>(Art.Hull, new Rectangle(Art.TileSize, 0, Art.TileSize, Art.TileSize), 0);
                }
                // Hull Inner Corner ES
                if (north != TileType.Hull && !EmptyTileTypes.Contains(north) && east == TileType.Hull && south == TileType.Hull && west != TileType.Hull && !EmptyTileTypes.Contains(west))
                {
                    return new Tuple<Texture2D, Rectangle, float>(Art.Hull, new Rectangle(Art.TileSize, 0, Art.TileSize, Art.TileSize), (float)Math.PI / 2);
                }
                // Hull Inner Corner SW
                if (north != TileType.Hull && !EmptyTileTypes.Contains(north) && east != TileType.Hull && !EmptyTileTypes.Contains(east) && south == TileType.Hull && west == TileType.Hull)
                {
                    return new Tuple<Texture2D, Rectangle, float>(Art.Hull, new Rectangle(Art.TileSize, 0, Art.TileSize, Art.TileSize), (float)Math.PI);
                }
                // Hull Inner Corner WN
                if (north == TileType.Hull && east != TileType.Hull && !EmptyTileTypes.Contains(east) && south != TileType.Hull && !EmptyTileTypes.Contains(south) && west == TileType.Hull)
                {
                    return new Tuple<Texture2D, Rectangle, float>(Art.Hull, new Rectangle(Art.TileSize, 0, Art.TileSize, Art.TileSize), -(float)Math.PI / 2);
                }
                // Hull Outer Corner NE
                if (north == TileType.Hull && east == TileType.Hull && EmptyTileTypes.Contains(south) && EmptyTileTypes.Contains(west))
                {
                    return new Tuple<Texture2D, Rectangle, float>(Art.Hull, new Rectangle(Art.TileSize * 2, 0, Art.TileSize, Art.TileSize), (float)Math.PI);
                }
                // Hull Outer Corner ES
                if (EmptyTileTypes.Contains(north) && east == TileType.Hull && south == TileType.Hull && EmptyTileTypes.Contains(west))
                {
                    return new Tuple<Texture2D, Rectangle, float>(Art.Hull, new Rectangle(Art.TileSize * 2, 0, Art.TileSize, Art.TileSize), -(float)Math.PI / 2);
                }
                // Hull Outer Corner SW
                if (EmptyTileTypes.Contains(north) && EmptyTileTypes.Contains(east) && south == TileType.Hull && west == TileType.Hull)
                {
                    return new Tuple<Texture2D, Rectangle, float>(Art.Hull, new Rectangle(Art.TileSize * 2, 0, Art.TileSize, Art.TileSize), 0);
                }
                // Hull Outer Corner WN
                if (north == TileType.Hull && EmptyTileTypes.Contains(east) && EmptyTileTypes.Contains(south) && west == TileType.Hull)
                {
                    return new Tuple<Texture2D, Rectangle, float>(Art.Hull, new Rectangle(Art.TileSize * 2, 0, Art.TileSize, Art.TileSize), (float)Math.PI / 2);
                }
                // Hull Triple North
                // Hull Triple South
                // Hull Triple West
                // Hull Triple East
            }
            return null;
        }
    }
}
