using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace StellarOps
{
    public static class Art
    {
        public static Texture2D TestShip { get; set; }
        public static Texture2D TestShipInterior { get; set; }
        public static Texture2D Player { get; set; }
        public static Texture2D Seeker { get; set; }
        public static Texture2D Wanderer { get; set; }
        public static Texture2D Bullet { get; set; }
        public static Texture2D Pointer { get; set; }
        public static Texture2D Background { get; set; }

        public static SpriteFont DebugFont { get; private set; }

        public static void Load(ContentManager content)
        {
            Player = content.Load<Texture2D>("Pawns/TestPawn1");
            Background = content.Load<Texture2D>("starfield2");
            Seeker = content.Load<Texture2D>("Seeker");
            Wanderer = content.Load<Texture2D>("Wanderer");
            Bullet = content.Load<Texture2D>("Bullet");
            Pointer = content.Load<Texture2D>("Pointer");
            TestShip = content.Load<Texture2D>("Ships/TestShip_Interior2");
            TestShipInterior = content.Load<Texture2D>("Ships/TestShip_Interior2");

            DebugFont = content.Load<SpriteFont>("DebugFont");
        }

        public static Texture2D DrawTileRectangle(int width, int height, Color fillColor, Color borderColor)
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
