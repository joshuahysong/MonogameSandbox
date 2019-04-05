using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StellarOps.Contracts;
using System;
using System.Collections.Generic;

namespace StellarOps
{
    public class Tile : Entity
    {
        public IContainer Container { get; set; }
        public Texture2D Image { get; set; }
        public Rectangle ImageSource { get; set; }
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
            // Check if neighbors destroyed
            if ((North == null || Container.Tiles[North.Value].Health <= 0)
                && (East == null || Container.Tiles[East.Value].Health <= 0)
                && (South == null || Container.Tiles[South.Value].Health <= 0)
                && (West == null || Container.Tiles[West.Value].Health <= 0))
            {
                Image = null;
                TileType = TileType.Empty;
            }
        }

        public override void Draw(SpriteBatch spriteBatch, Matrix parentTransform)
        {
            if (Image != null 
                && !((TileType == TileType.MainThrust && !Container.IsMainThrustFiring)
                || (TileType == TileType.PortThrust && !Container.IsPortThrustFiring)
                || (TileType == TileType.StarboardThrust && !Container.IsStarboardThrustFiring)))
            {
                Matrix globalTransform = LocalTransform * parentTransform;
                DecomposeMatrix(ref globalTransform, out Vector2 position, out float rotation, out Vector2 scale);

                spriteBatch.Draw(Image, position, ImageSource, Color.White, rotation, DrawCenter, scale * MainGame.TileScale, SpriteEffects.None, 0.0f);

                if (TileType != TileType.Empty)
                {
                    if (Health < 100 && Health >= 75)
                    {
                        spriteBatch.Draw(Art.Damage25, position, null, Color.White, rotation, DrawCenter, scale * MainGame.TileScale, SpriteEffects.None, 0.0f);
                    }
                    if (Health < 75 && Health >= 50)
                    {
                        spriteBatch.Draw(Art.Damage50, position, null, Color.White, rotation, DrawCenter, scale * MainGame.TileScale, SpriteEffects.None, 0.0f);
                    }
                    if (Health < 50)
                    {
                        spriteBatch.Draw(Art.Damage75, position, null, Color.White, rotation, DrawCenter, scale * MainGame.TileScale, SpriteEffects.None, 0.0f);
                    }
               }
            }
        }
    }
}
