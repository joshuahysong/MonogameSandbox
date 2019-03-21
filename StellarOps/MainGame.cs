using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StellarOps.Ships;
using System;
using System.Collections.Generic;

namespace StellarOps
{
    public class MainGame : Game
    {
        public static MainGame Instance { get; private set; }
        public static Camera Camera { get; set; }
        public static Player Player { get; set; }
        public static ShipCore Ship { get; set; }
        public static bool IsDebugging { get; set; }

        public static Viewport Viewport => Instance.GraphicsDevice.Viewport;
        public static Vector2 ScreenSize => new Vector2(Viewport.Width, Viewport.Height);
        public static Vector2 ScreenCenter => new Vector2(Viewport.Width / 2, Viewport.Height / 2);

        public const int WorldTileSize = 4000;
        public const int TileSize =  32;
        public const float TileScale = 0.5f;
        public const float PawnScale = 0.4f;

        public Dictionary<string, string> PlayerDebugEntries { get; set; }
        public Dictionary<string, string> ShipDebugEntries { get; set; }
        public Dictionary<string, string> SystemDebugEntries { get; set; }

        private Texture2D debugTile;
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private Vector2 tilePosition;

        public MainGame()
        {
            graphics = new GraphicsDeviceManager(this)
            {
                IsFullScreen = false,
                PreferredBackBufferHeight = 900,
                PreferredBackBufferWidth = 1440
            };
            Content.RootDirectory = "Content";
            Instance = this;
            IsDebugging = true;
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            Camera = new Camera();
            debugTile = Art.CreateRectangle(WorldTileSize, WorldTileSize, Color.TransparentBlack, Color.DimGray * 0.5f);
            PlayerDebugEntries = new Dictionary<string, string>();
            ShipDebugEntries = new Dictionary<string, string>();
            SystemDebugEntries = new Dictionary<string, string>();

            base.Initialize();
            Player = new Player();
            Ship = new TestShip2(Vector2.Zero);
            Ship.Pawns.Add(Player);
            Player.Container = Ship;
            Camera.Focus = Ship;
            EntityManager.Add(Ship);
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Art.Load(Content);
        }

        protected override void Update(GameTime gameTime)
        {
            Input.Update();
            HandleInput();
            Camera.HandleInput();

            Camera.Update(Camera.Focus);
            EntityManager.Update(gameTime, Matrix.Identity);
            base.Update(gameTime);

            if (IsDebugging)
            {
                SystemDebugEntries["FPS"] = $"{Math.Round(1 / gameTime.ElapsedGameTime.TotalSeconds)}";
                SystemDebugEntries["Camera Focus"] = $"{Camera.Focus.GetType().Name}";
                SystemDebugEntries["Camera Zoom"] = $"{Math.Round(Camera.Scale,2)}";
                SystemDebugEntries["Mouse Screen Position"] = $"{Input.ScreenMousePosition.X}, {Input.ScreenMousePosition.Y}";
                SystemDebugEntries["Mouse World Position"] = $"{Math.Round(Input.WorldMousePosition.X)}, {Math.Round(Input.WorldMousePosition.Y)}";
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, Camera.Transform);
            DrawBackground(spriteBatch);
            EntityManager.Draw(spriteBatch, Matrix.Identity);
            spriteBatch.End();

            spriteBatch.Begin();
            //spriteBatch.Draw(Art.Pointer, new Vector2(Input.ScreenMousePosition.X, Input.ScreenMousePosition.Y), Color.White);

            // Player prompt text
            if (Camera.Focus == Player)
            {
                string promptText = Player.Container.GetUsePrompt(Player.Position);
                if (!string.IsNullOrWhiteSpace(promptText))
                {
                    Vector2 textSize = Art.DebugFont.MeasureString(promptText);
                    Vector2 textLocation = new Vector2(ScreenCenter.X - textSize.X / 2, ScreenCenter.Y + Player.Radius * 2f);
                    spriteBatch.Draw(Art.Pixel, new Rectangle((int)textLocation.X - 3, (int)textLocation.Y - 3, (int)textSize.X + 6, (int)textSize.Y + 6), Color.DarkCyan * 0.9f);
                    spriteBatch.DrawString(Art.DebugFont, promptText, textLocation, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
                }
            }

            // Debug Text
            if (IsDebugging)
            {
                int xTextOffset = 5;
                int yTextOffset = 5;
                spriteBatch.DrawString(Art.DebugFont, "Player", new Vector2(xTextOffset, yTextOffset), Color.White);
                foreach (KeyValuePair<string, string> debugEntry in PlayerDebugEntries)
                {
                    yTextOffset += 15;
                    spriteBatch.DrawString(Art.DebugFont, $"{debugEntry.Key}: {debugEntry.Value}", new Vector2(xTextOffset, yTextOffset), Color.White);
                }

                xTextOffset = Viewport.Width / 2 - 200;
                yTextOffset = 5;
                spriteBatch.DrawString(Art.DebugFont, "Ship", new Vector2(xTextOffset, yTextOffset), Color.White);
                foreach (KeyValuePair<string, string> debugEntry in ShipDebugEntries)
                {
                    yTextOffset += 15;
                    spriteBatch.DrawString(Art.DebugFont, $"{debugEntry.Key}: {debugEntry.Value}", new Vector2(xTextOffset, yTextOffset), Color.White);
                }

                yTextOffset = 5;
                spriteBatch.DrawString(Art.DebugFont, "Ship", new Vector2(xTextOffset, yTextOffset), Color.White);
                foreach (KeyValuePair<string, string> debugEntry in SystemDebugEntries)
                {
                    string text = $"{debugEntry.Key}: {debugEntry.Value}";
                    xTextOffset = (int)(Viewport.Width - 5 - Art.DebugFont.MeasureString(text).X);
                    spriteBatch.DrawString(Art.DebugFont, $"{debugEntry.Key}: {debugEntry.Value}", new Vector2(xTextOffset, yTextOffset), Color.White);
                    yTextOffset += 15;
                }
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }

        private void HandleInput()
        {
            if (Input.WasKeyPressed(Keys.Escape))
            {
                Exit();
            }
            if (Input.WasKeyPressed(Keys.F4))
            {
                IsDebugging = !IsDebugging;
            }
        }

        public Texture2D DrawStars(Texture2D tile)
        {
            Random Random = new Random();
            Color[] data = new Color[tile.Width * tile.Height];
            tile.GetData(data);
            for (int i = 0; i < data.Length; ++i)
            {
                if (!(i < tile.Width || i % tile.Width == 0 || i > tile.Width * tile.Width - tile.Width || (i + 1) % tile.Width == 0))
                {
                    if (Random.Next(1, 50000) == 1)
                    {
                        data[i] = Color.White;
                    }
                    else if (Random.Next(1, 50000) == 1)
                    {
                        data[i] = Color.DimGray;
                    }
                }
            }
            tile.SetData(data);
            return tile;
        }

        private void DrawBackground(SpriteBatch spriteBatch)
        {
            Vector2 startLocation = Vector2.Zero;
            int numberOfTilesX = (int)Math.Ceiling((double)Viewport.Bounds.Width / WorldTileSize / Camera.Scale);
            int numberOfTilesY = (int)Math.Ceiling((double)Viewport.Bounds.Height / WorldTileSize / Camera.Scale);
            tilePosition.X = (int)Math.Floor(Camera.Position.X / WorldTileSize);
            tilePosition.Y = (int)Math.Floor(Camera.Position.Y / WorldTileSize);

            startLocation.X = startLocation.X- WorldTileSize;

            int minX = (int)Math.Floor(tilePosition.X - (double)numberOfTilesX / 2);
            int maxX = (int)Math.Ceiling(tilePosition.X + (double)numberOfTilesX / 2);
            int minY = (int)Math.Floor(tilePosition.Y - (double)numberOfTilesY / 2);
            int maxY = (int)Math.Ceiling(tilePosition.Y + (double)numberOfTilesY / 2);

            for (int x = minX; x <= maxX; x++) 
            {
                for (int y = minY; y <= maxY; y++)
                {
                    Rectangle backgroundRectangle = new Rectangle(WorldTileSize * x, WorldTileSize * y, WorldTileSize, WorldTileSize);
                    spriteBatch.Draw(Art.Background, backgroundRectangle, Color.White);

                    DrawTile(x, y);
                }
            }
        }

        private void DrawTile(int x, int y)
        {
            //Vector2 position = new Vector2(starTile.Bounds.Width * x, starTile.Bounds.Height * y);
            //spriteBatch.Draw(starTile, position, Color.White);
            Vector2 position = new Vector2(debugTile.Bounds.Width * x, debugTile.Bounds.Height * y);
            if (IsDebugging && Camera.Scale >= 0.5)
            {
                spriteBatch.Draw(debugTile, position, Color.White);
                spriteBatch.DrawString(Art.DebugFont, $"{x},{y}", new Vector2(position.X + 5, position.Y + 5), Color.DimGray);
            }
        }
    }
}
