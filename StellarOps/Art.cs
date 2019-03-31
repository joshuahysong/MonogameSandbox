using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace StellarOps
{
    public static class Art
    {
        public static int TileSize => 64;

        public static Texture2D Pixel { get; set; }
        public static Texture2D TestShip { get; set; }
        public static Texture2D TestShipInterior { get; set; }
        public static Texture2D Player { get; set; }
        public static Texture2D Seeker { get; set; }
        public static Texture2D Wanderer { get; set; }
        public static Texture2D Bullet { get; set; }
        public static Texture2D Pointer { get; set; }
        public static Texture2D Background { get; set; }

        #region ShipTiles
        public static Texture2D Damage25 { get; set; }
        public static Texture2D Damage50 { get; set; }
        public static Texture2D Damage75 { get; set; }
        public static Texture2D Hull { get; set; }
        public static Texture2D HullTest { get; set; }
        public static Texture2D Floor { get; set; }
        public static Texture2D FlightConsole { get; set; }
        public static Texture2D Engine { get; set; }
        public static Texture2D Weapon { get; set; }
        public static Texture2D MainThruster { get; set; }
        #endregion

        #region Fonts
        public static SpriteFont DebugFont { get; private set; }
        public static SpriteFont UIFont { get; private set; }
        #endregion

        public static void Load(ContentManager content)
        {
            Pixel = CreateRectangle(1, 1, Color.White, Color.White);
            Player = content.Load<Texture2D>("Pawns/TestPawn3");
            Background = content.Load<Texture2D>("starfield2");
            Bullet = CreateRectangle(10, 10, Color.White, Color.White);

            Damage25 = content.Load<Texture2D>("Tiles/DamageOverlay25");
            Damage50 = content.Load<Texture2D>("Tiles/DamageOverlay50");
            Damage75 = content.Load<Texture2D>("Tiles/DamageOverlay75");
            Hull = content.Load<Texture2D>("Tiles/Hull");
            HullTest = content.Load<Texture2D>("Tiles/HullTest");
            Floor = content.Load<Texture2D>("Tiles/Floor");
            FlightConsole = content.Load<Texture2D>("Tiles/FlightConsole");
            Engine = CreateRectangle(TileSize, TileSize, Color.SaddleBrown, Color.Brown);
            Weapon = content.Load<Texture2D>("Tiles/Weapon");
            MainThruster = CreateRectangle(TileSize, TileSize, Color.LightCyan, Color.LightCyan);

            DebugFont = content.Load<SpriteFont>("Fonts/Debug");
            UIFont = content.Load<SpriteFont>("Fonts/UI");
        }

        public static Texture2D CreateRectangle(int width, int height, Color fillColor, Color borderColor)
        {
            Texture2D tile = new Texture2D(MainGame.Instance.GraphicsDevice, width, height);
            Color[] data = new Color[width * height];
            for (int i = 0; i < data.Length; ++i)
            {
                if (i < width || i % width == 0 || i > width * height - width || (i + 1) % width == 0)
                {
                    data[i] = borderColor;
                }
                else
                {
                    data[i] = fillColor;
                }
            }
            tile.SetData(data);
            return tile;
        }

        public static Texture2D CreateCircle(int radius, Color borderColor)
        {
            int outerRadius = radius * 2 + 2; // So circle doesn't go out of bounds
            Texture2D texture = new Texture2D(MainGame.Instance.GraphicsDevice, outerRadius, outerRadius);

            Color[] data = new Color[outerRadius * outerRadius];

            // Colour the entire texture transparent first.
            for (int i = 0; i < data.Length; i++)
                data[i] = Color.TransparentBlack;

            // Work out the minimum step necessary using trigonometry + sine approximation.
            double angleStep = 1f / radius;

            for (double angle = 0; angle < Math.PI * 2; angle += angleStep)
            {
                // Use the parametric definition of a circle: http://en.wikipedia.org/wiki/Circle#Cartesian_coordinates
                int x = (int)Math.Round(radius + radius * Math.Cos(angle));
                int y = (int)Math.Round(radius + radius * Math.Sin(angle));

                data[y * outerRadius + x + 1] = borderColor;
            }

            texture.SetData(data);
            return texture;
        }
    }
}
