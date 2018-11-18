using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace StellarOps
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class MainGame : Game
    {
        public Vector2 CenterScreen { get; set; }
        public static MainGame Instance { get; private set; }
        public static Viewport Viewport { get { return Instance.GraphicsDevice.Viewport; } }
        public static Vector2 ScreenSize { get { return new Vector2(Viewport.Width, Viewport.Height); } }
        public static Camera Camera { get; set; }

        private int TileSize => 300;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D background;
        SpriteFont debugFont;
        Viewport viewport;
        Player player;
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

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            CenterScreen = new Vector2(GraphicsDevice.Viewport.Bounds.Width / 2, GraphicsDevice.Viewport.Bounds.Height / 2);
            viewport = GraphicsDevice.Viewport;
            player = new Player
            {
                Position = Vector2.Zero,
                Heading = 0.0f,
                Thrust = 500f,
                TurnRate = 0.05f
            };
            Camera = new Camera(this);
            Camera.Initialize();
            testTile = DrawTileRectangle(Color.TransparentBlack);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            player.Ship = Content.Load<Texture2D>("ship");
            background = Content.Load<Texture2D>("starfield");
            debugFont = Content.Load<SpriteFont>("debug");
            Art.Load(Content);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();

            this.HandleInput(keyboardState);
            player.HandleInput(keyboardState);
            player.Update(gameTime);
            Camera.Update(player);
            EntityManager.Update(gameTime);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            viewport = GraphicsDevice.Viewport;

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, Camera.Transform);
            DrawBackground(spriteBatch);
            player.Draw(spriteBatch);
            EntityManager.Draw(spriteBatch);
            spriteBatch.End();

            spriteBatch.Begin();
            spriteBatch.DrawString(debugFont, $"Position: {Math.Round(player.Position.X)}, {Math.Round(player.Position.Y)}", new Vector2(viewport.Bounds.X + 5, viewport.Bounds.Y + 5), Color.White);
            spriteBatch.DrawString(debugFont, $"Velocity : {Math.Round(player.Velocity.X)}, {Math.Round(player.Velocity.Y)}", new Vector2(viewport.Bounds.X + 5, viewport.Bounds.Y + 25), Color.White);
            spriteBatch.DrawString(debugFont, $"Heading : {Math.Round(player.Heading, 2)}", new Vector2(viewport.Bounds.X + 5, viewport.Bounds.Y + 45), Color.White);
            spriteBatch.DrawString(debugFont, $"Tile : {tilePosition.X}, {tilePosition.Y}", new Vector2(viewport.Bounds.X + 5, viewport.Bounds.Y + 65), Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void HandleInput(KeyboardState keyboardState)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || keyboardState.IsKeyDown(Keys.Escape))
                Exit();
        }

        private Texture2D testTile;

        private Texture2D DrawTileRectangle(Color color)
        {
            var tile = new Texture2D(GraphicsDevice, TileSize, TileSize);
            Color[] data = new Color[TileSize * TileSize];
            for (int i = 0; i < data.Length; ++i)
            {
                if (i < TileSize || i % TileSize == 0 || i > TileSize * TileSize - TileSize || (i + 1) % TileSize == 0)
                {
                    data[i] = Color.DimGray;
                }
                else
                {
                    data[i] = color;
                }
            }
            tile.SetData(data);
            return tile;
        }

        private void DrawBackground(SpriteBatch spriteBatch)
        {
            Vector2 startLocation = Vector2.Zero;

            int numberOfTilesX = (int)Math.Ceiling((decimal)viewport.Bounds.Width / TileSize);
            int numberOfTilesY = (int)Math.Ceiling((decimal)viewport.Bounds.Height / TileSize);
            tilePosition.X = (int)Math.Floor((player.Position.X) / TileSize);
            tilePosition.Y = (int)Math.Floor((player.Position.Y) / TileSize);

            spriteBatch.Draw(testTile, startLocation, Color.White);
            startLocation.X = startLocation.X- TileSize;
            DrawTile(-1, 0);

            int minX = (int)Math.Floor(tilePosition.X - (double)numberOfTilesX / 2);
            int maxX = (int)Math.Ceiling(tilePosition.X + (double)numberOfTilesX / 2);
            int minY = (int)Math.Floor(tilePosition.Y - (double)numberOfTilesY / 2);
            int maxY = (int)Math.Ceiling(tilePosition.Y + (double)numberOfTilesY / 2);

            for (int x = minX; x <= maxX; x++) 
            {
                for (int y = minY; y <= maxY; y++)
                {
                    DrawTile(x, y);
                }
            }
        }

        private void DrawTile(int x, int y)
        {
            var position = new Vector2(testTile.Bounds.Width * x, testTile.Bounds.Height * y);
            spriteBatch.Draw(testTile, position, Color.White);
            spriteBatch.DrawString(debugFont, $"{x},{y}", new Vector2(position.X + 5, position.Y + 5), Color.DimGray);
        }
    }
}
