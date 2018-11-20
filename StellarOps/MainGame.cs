using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace StellarOps
{
    public class MainGame : Game
    {
        public static MainGame Instance { get; private set; }
        public static Viewport Viewport => Instance.GraphicsDevice.Viewport;
        public static Vector2 ScreenSize => new Vector2(Viewport.Width, Viewport.Height);
        public static Vector2 ScreenCenter => new Vector2(Viewport.Width / 2, Viewport.Height / 2);
        public static Random Random = new Random();
        public static bool IsDebugging = false;
        public static Player Player;
        public Camera Camera { get; set; }

        private int TileSize => 1000;
        private Texture2D starTile;
        private Texture2D debugTile;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Vector2 tilePosition;

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
        }

        protected override void Initialize()
        {
            Camera = new Camera();
            starTile = DrawTileRectangle(Color.TransparentBlack, false);
            debugTile = DrawTileRectangle(Color.TransparentBlack, true);

            base.Initialize();
            Player = new Player
            {
                Position = Vector2.Zero,
                Heading = 0.0f,
                Thrust = 500f,
                TurnRate = 0.05f
            };
            EntityManager.Add(Player);
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Art.Load(Content);
        }

        protected override void Update(GameTime gameTime)
        {
            Input.Update();
            this.HandleInput();
            Camera.HandleInput();
            Player.HandleInput();

            Camera.Update(Player);
            EntityManager.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, Camera.Transform);
            DrawBackground(spriteBatch);
            if (IsDebugging)
            {
                DrawBackground(spriteBatch, true);
            }
            EntityManager.Draw(spriteBatch);
            spriteBatch.End();

            spriteBatch.Begin();
            spriteBatch.Draw(Art.Pointer, new Vector2(Input.MouseState.X, Input.MouseState.Y), Color.White);
            if (IsDebugging)
            {
                spriteBatch.DrawString(Art.DebugFont, $"Position: {Math.Round(Player.Position.X)}, {Math.Round(Player.Position.Y)}", new Vector2(Viewport.Bounds.X + 5, Viewport.Bounds.Y + 5), Color.White);
                spriteBatch.DrawString(Art.DebugFont, $"Velocity : {Math.Round(Player.Velocity.X)}, {Math.Round(Player.Velocity.Y)}", new Vector2(Viewport.Bounds.X + 5, Viewport.Bounds.Y + 25), Color.White);
                spriteBatch.DrawString(Art.DebugFont, $"Heading : {Math.Round(Player.Heading, 2)}", new Vector2(Viewport.Bounds.X + 5, Viewport.Bounds.Y + 45), Color.White);
                spriteBatch.DrawString(Art.DebugFont, $"Tile : {tilePosition.X}, {tilePosition.Y}", new Vector2(Viewport.Bounds.X + 5, Viewport.Bounds.Y + 65), Color.White);
                spriteBatch.DrawString(Art.DebugFont, $"Zoom : {Camera.Scale}", new Vector2(Viewport.Bounds.X + 5, Viewport.Bounds.Y + 85), Color.White);
                spriteBatch.DrawString(Art.DebugFont, $"Mouse : {Input.MouseState.X}, {Input.MouseState.Y}", new Vector2(Viewport.Bounds.X + 5, Viewport.Bounds.Y + 105), Color.White);
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

        private Texture2D DrawTileRectangle(Color color, bool border = false)
        {
            Texture2D tile = new Texture2D(GraphicsDevice, TileSize, TileSize);
            Color[] data = new Color[TileSize * TileSize];
            for (int i = 0; i < data.Length; ++i)
            {
                if (border && (i < TileSize || i % TileSize == 0 || i > TileSize * TileSize - TileSize || (i + 1) % TileSize == 0))
                {
                    data[i] = Color.DimGray;
                }
                else
                {
                    if (!border && Random.Next(1, 5000) == 1)
                    {
                        data[i] = Color.White;
                    }
                    else
                    {
                        data[i] = color;
                    }
                }
            }
            tile.SetData(data);
            return tile;
        }

        private void DrawBackground(SpriteBatch spriteBatch, bool areDrawingDebugTiles = false)
        {
            Vector2 startLocation = Vector2.Zero;
            int numberOfTilesX = (int)Math.Ceiling(((double)Viewport.Bounds.Width / TileSize) / Camera.Scale);
            int numberOfTilesY = (int)Math.Ceiling(((double)Viewport.Bounds.Height / TileSize) / Camera.Scale);
            tilePosition.X = (int)Math.Floor((Player.Position.X) / TileSize);
            tilePosition.Y = (int)Math.Floor((Player.Position.Y) / TileSize);

            startLocation.X = startLocation.X- TileSize;

            int minX = (int)Math.Floor(tilePosition.X - (double)numberOfTilesX / 2);
            int maxX = (int)Math.Ceiling(tilePosition.X + (double)numberOfTilesX / 2);
            int minY = (int)Math.Floor(tilePosition.Y - (double)numberOfTilesY / 2);
            int maxY = (int)Math.Ceiling(tilePosition.Y + (double)numberOfTilesY / 2);

            for (int x = minX; x <= maxX; x++) 
            {
                for (int y = minY; y <= maxY; y++)
                {
                    DrawTile(x, y, areDrawingDebugTiles);
                }
            }
        }

        private void DrawTile(int x, int y, bool IsDrawingDebugTile)
        {
            Texture2D tileToDraw = starTile;
            if (IsDrawingDebugTile)
            {
                tileToDraw = debugTile;
            }
            Vector2 position = new Vector2(tileToDraw.Bounds.Width * x, tileToDraw.Bounds.Height * y);
            spriteBatch.Draw(tileToDraw, position, Color.White);
            if (IsDebugging)
            {
                spriteBatch.DrawString(Art.DebugFont, $"{x},{y}", new Vector2(position.X + 5, position.Y + 5), Color.DimGray);
            }
        }
    }
}
