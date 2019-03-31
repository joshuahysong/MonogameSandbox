using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StellarOps.Contracts;
using System;

namespace StellarOps
{
    public class Tile : Entity
    {
        public IContainer Container { get; set; }
        public Texture2D Image { get; set; }
        public Point Location { get; set; }
        public Rectangle Bounds { get; set; }
        public CollisionType CollisionType { get; set; }
        public TileType TileType { get; set; }
        public int Health { get; set; }
        public int? North { get; set; }
        public int? NorthEast { get; set; }
        public int? East { get; set; }
        public int? SouthEast { get; set; }
        public int? South { get; set; }
        public int? SouthWest { get; set; }
        public int? West { get; set; }
        public int? NorthWest { get; set; }

        public Vector2 DrawCenter = new Vector2(Art.TileSize / 2, Art.TileSize / 2);
        public Vector2 TileCenter = new Vector2(MainGame.TileSize / 2, MainGame.TileSize / 2);

        public override void Update(GameTime gameTime, Matrix parentTransform)
        {
        }

        public override void Draw(SpriteBatch spriteBatch, Matrix parentTransform)
        {
            Tuple<Texture2D, Rectangle, float> tileArtData = GetTileImage();
            if (tileArtData != null)
            {
                Image = tileArtData.Item1;
                Matrix globalTransform = LocalTransform * parentTransform;
                DecomposeMatrix(ref globalTransform, out Vector2 position, out float rotation, out Vector2 scale);
                spriteBatch.Draw(Image, position, tileArtData.Item2, Color.White, rotation - tileArtData.Item3, DrawCenter, scale * MainGame.TileScale, SpriteEffects.None, 0.0f);
            }
        }

        private Tuple<Texture2D, Rectangle, float> GetTileImage()
        {
            if (TileType == TileType.Weapon)
            {
                return new Tuple<Texture2D, Rectangle, float>(Art.Weapon, Art.Weapon.Bounds, 0);
            }
            if (TileType == TileType.Engine)
            {
                return new Tuple<Texture2D, Rectangle, float>(Art.Engine, Art.Engine.Bounds, 0);
            }
            if (TileType == TileType.Floor)
            {
                return new Tuple<Texture2D, Rectangle, float>(Art.Floor, Art.Floor.Bounds, 0);
            }
            if (TileType == TileType.FlightConsole)
            {
                return new Tuple<Texture2D, Rectangle, float>(Art.FlightConsole, Art.FlightConsole.Bounds, 0);
            }
            if ((TileType == TileType.MainThrust && Container.IsMainThrustFiring)
                || (TileType == TileType.PortThrust && Container.IsPortThrustFiring)
                || (TileType == TileType.StarboardThrust && Container.IsStarboardThrustFiring))
            {
                return new Tuple<Texture2D, Rectangle, float>(Art.MainThruster, Art.MainThruster.Bounds, 0);
            }
            if (TileType == TileType.Hull)
            {
                bool north = North == null ? false : Container.Tiles[(int)North].TileType == TileType.Hull;
                bool east = East == null ? false : Container.Tiles[(int)East].TileType == TileType.Hull;
                bool south = South == null ? false : Container.Tiles[(int)South].TileType == TileType.Hull;
                bool west = West == null ? false : Container.Tiles[(int)West].TileType == TileType.Hull;
                //if (north && east && south && west)
                //{
                //    return Art.HullFull;
                //}
                if (north && east && !south && !west)
                {
                    return new Tuple<Texture2D, Rectangle, float>(Art.Hull, new Rectangle(3 * Art.TileSize, 1 * Art.TileSize, Art.TileSize, Art.TileSize), 0);
                }
                if (!north && east && south && !west)
                {
                    return new Tuple<Texture2D, Rectangle, float>(Art.Hull, new Rectangle(2 * Art.TileSize, 1 * Art.TileSize, Art.TileSize, Art.TileSize), 0);
                }
                if (!north && !east && south && west)
                {
                    return new Tuple<Texture2D, Rectangle, float>(Art.Hull, new Rectangle(0 * Art.TileSize, 2 * Art.TileSize, Art.TileSize, Art.TileSize), 0);
                }
                if (north && !east && !south && west)
                {
                    return new Tuple<Texture2D, Rectangle, float>(Art.Hull, new Rectangle(1 * Art.TileSize, 2 * Art.TileSize, Art.TileSize, Art.TileSize), 0);
                }
                if (north && !east && south && !west)
                {
                    return new Tuple<Texture2D, Rectangle, float>(Art.Hull, new Rectangle(0 * Art.TileSize, 0 * Art.TileSize, Art.TileSize, Art.TileSize), (float)Math.PI / 2);
                    //return new Tuple<Texture2D, Rectangle, float>(Art.Hull, new Rectangle(1 * Art.TileSize, 0 * Art.TileSize, Art.TileSize, Art.TileSize) 0);
                }
                if (!north && east && !south && west)
                {
                    return new Tuple<Texture2D, Rectangle, float>(Art.Hull, new Rectangle(0 * Art.TileSize, 0 * Art.TileSize, Art.TileSize, Art.TileSize), 0);
                }
                if (north && !east && !south && !west)
                {
                    return new Tuple<Texture2D, Rectangle, float>(Art.Hull, new Rectangle(0 * Art.TileSize, 1 * Art.TileSize, Art.TileSize, Art.TileSize), 0);
                }
                if (!north && east && !south && !west)
                {
                    return new Tuple<Texture2D, Rectangle, float>(Art.Hull, new Rectangle(2 * Art.TileSize, 0 * Art.TileSize, Art.TileSize, Art.TileSize), 0);
                }
                if (!north && !east && south && !west)
                {
                    return new Tuple<Texture2D, Rectangle, float>(Art.Hull, new Rectangle(1 * Art.TileSize, 1 * Art.TileSize, Art.TileSize, Art.TileSize), 0);
                }
                if (!north && !east && !south && west)
                {
                    return new Tuple<Texture2D, Rectangle, float>(Art.Hull, new Rectangle(3 * Art.TileSize, 0 * Art.TileSize, Art.TileSize, Art.TileSize), 0);
                }
            }
            return null;// new Tuple<Texture2D, Rectangle, float>(Art.Floor, new Rectangle(0, 0, 0, 0), 0);
        }
    }
}
