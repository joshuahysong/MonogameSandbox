using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StellarOps.Ships;
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
        public static ShipCore Ship;
        public static Camera Camera { get; set; }

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
            starTile = DrawStars(DrawTileRectangle(TileSize, TileSize, Color.TransparentBlack, Color.TransparentBlack));
            debugTile = DrawTileRectangle(TileSize, TileSize, Color.TransparentBlack, Color.DimGray * 0.5f);

            base.Initialize();
            Player = new Player();
            Ship = new TestShip(Vector2.Zero);
            Player.Parent = Ship;
            Camera.Focus = Ship;
            Ship.Children.Add(Player);
            Player.Container = Ship;
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
            this.HandleInput();
            Camera.HandleInput();

            Camera.Update(Camera.Focus);
            EntityManager.Update(gameTime, Matrix.Identity);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, Camera.Transform);
            DrawBackground(spriteBatch);
            EntityManager.Draw(spriteBatch, Matrix.Identity);
            if (IsDebugging)
            {
                DrawBackground(spriteBatch, true);
            }
            spriteBatch.End();

            spriteBatch.Begin();
            spriteBatch.Draw(Art.Pointer, new Vector2(Input.MouseState.X, Input.MouseState.Y), Color.White);
            if (IsDebugging)
            {
                Entity focus = (Entity)Camera.Focus;
                float velocityHeading = (float)Math.Atan2(focus.Velocity.Y, focus.Velocity.X);
                // Left Topfocus
                spriteBatch.DrawString(Art.DebugFont, $"Position: {Math.Round(focus.Position.X)}, {Math.Round(focus.Position.Y)}", new Vector2(Viewport.Bounds.X + 5, Viewport.Bounds.Y + 5), Color.White);
                spriteBatch.DrawString(Art.DebugFont, $"Velocity: {Math.Round(focus.Velocity.X)}, {Math.Round(focus.Velocity.Y)}", new Vector2(Viewport.Bounds.X + 5, Viewport.Bounds.Y + 20), Color.White);
                spriteBatch.DrawString(Art.DebugFont, $"Heading: {Math.Round(focus.Heading, 2)}", new Vector2(Viewport.Bounds.X + 5, Viewport.Bounds.Y + 35), Color.White);
                spriteBatch.DrawString(Art.DebugFont, $"Velocity Heading: {Math.Round(velocityHeading, 2)}", new Vector2(Viewport.Bounds.X + 5, Viewport.Bounds.Y + 50), Color.White);
                spriteBatch.DrawString(Art.DebugFont, $"Tile: {tilePosition.X}, {tilePosition.Y}", new Vector2(Viewport.Bounds.X + 5, Viewport.Bounds.Y + 65), Color.White);
                spriteBatch.DrawString(Art.DebugFont, $"Zoom: {Camera.Scale}", new Vector2(Viewport.Bounds.X + 5, Viewport.Bounds.Y + 80), Color.White);
                spriteBatch.DrawString(Art.DebugFont, $"Mouse Screen Position: \r {Input.ScreenMousePosition.X}, {Input.ScreenMousePosition.Y}", new Vector2(Viewport.Bounds.X + 5, Viewport.Bounds.Y + 95), Color.White);
                spriteBatch.DrawString(Art.DebugFont, $"Mouse World Position: \r {Input.WorldMousePosition.X}, {Input.WorldMousePosition.Y}", new Vector2(Viewport.Bounds.X + 5, Viewport.Bounds.Y + 110), Color.White);
                // Right Top
                string fps = $"FPS : {Math.Round(1 / gameTime.ElapsedGameTime.TotalSeconds)}";
                spriteBatch.DrawString(Art.DebugFont, fps, new Vector2(Viewport.Width - 5 - Art.DebugFont.MeasureString(fps).X, Viewport.Bounds.Y + 5), Color.White);
                string focusName = $"Camera Focus: {Camera.Focus.GetType().Name}";
                spriteBatch.DrawString(Art.DebugFont, focusName, new Vector2(Viewport.Width - 5 - Art.DebugFont.MeasureString(focusName).X, Viewport.Bounds.Y + 20), Color.White);
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

        public Texture2D DrawTileRectangle(int width, int height, Color fillColor, Color borderColor)
        {
            Texture2D tile = new Texture2D(GraphicsDevice, width, height);
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

        public Texture2D DrawStars(Texture2D tile)
        {
            Color[] data = new Color[tile.Width * tile.Height];
            tile.GetData(data);
            for (int i = 0; i < data.Length; ++i)
            {
                if (!(i < tile.Width || i % tile.Width == 0 || i > tile.Width * tile.Width - tile.Width || (i + 1) % tile.Width == 0))
                {
                    if (Random.Next(1, 10000) == 1)
                    {
                        data[i] = Color.White;
                    }
                    else if (Random.Next(1, 10000) == 1)
                    {
                        data[i] = Color.DimGray;
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
            tilePosition.X = (int)Math.Floor(Camera.Position.X / TileSize);
            tilePosition.Y = (int)Math.Floor(Camera.Position.Y / TileSize);

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
