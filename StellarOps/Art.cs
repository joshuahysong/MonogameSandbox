using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

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
            Player = content.Load<Texture2D>("ship3");
            Background = content.Load<Texture2D>("Pointer");
            Seeker = content.Load<Texture2D>("Seeker");
            Wanderer = content.Load<Texture2D>("Wanderer");
            Bullet = content.Load<Texture2D>("Bullet");
            Pointer = content.Load<Texture2D>("Pointer");
            TestShip = content.Load<Texture2D>("Ships/TestShip");
            TestShipInterior = content.Load<Texture2D>("Ships/TestShip_Interior");

            DebugFont = content.Load<SpriteFont>("DebugFont");
        }
    }
}
