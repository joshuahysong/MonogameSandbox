using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace StellarOps
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class MainGame : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D background;
        SpriteFont debugFont;
        Viewport viewport;
        Player player;
        Camera camera;
        int playerTileX;
        int playerTileY;

        public Vector2 CenterScreen;

        public MainGame()
        {
            graphics = new GraphicsDeviceManager(this)
            {
                IsFullScreen = false,
                PreferredBackBufferHeight = 900,
                PreferredBackBufferWidth = 1440
            };
            Content.RootDirectory = "Content";
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
            player = new Player(Vector2.Zero);
            camera = new Camera(this);
            camera.Initialize();
            testTile1 = DrawTileRectangle(200, Color.DarkCyan);
            testTile2 = DrawTileRectangle(200, Color.Brown);
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
            camera.Update(player);

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

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.Transform);
            TestDrawBackground(spriteBatch);
            player.Draw(spriteBatch);
            spriteBatch.End();

            spriteBatch.Begin();
            spriteBatch.DrawString(debugFont, $"Position: {Math.Round(player.Position.X)}, {Math.Round(player.Position.Y)}", new Vector2(viewport.Bounds.X + 5, viewport.Bounds.Y + 5), Color.White);
            spriteBatch.DrawString(debugFont, $"Heading : {Math.Round(player.Heading,2)}", new Vector2(viewport.Bounds.X + 5, viewport.Bounds.Y + 25), Color.White);
            spriteBatch.DrawString(debugFont, $"Tile : {playerTileX}, {playerTileY}", new Vector2(viewport.Bounds.X + 5, viewport.Bounds.Y + 45), Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void HandleInput(KeyboardState keyboardState)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || keyboardState.IsKeyDown(Keys.Escape))
                Exit();
        }

        private Texture2D testTile1;
        private Texture2D testTile2;

        private Texture2D DrawTileRectangle(int tileSize, Color color)
        {
            var tile = new Texture2D(GraphicsDevice, tileSize, tileSize);
            Color[] data = new Color[tileSize * tileSize];
            for (int i = 0; i < data.Length; ++i)
            {
                if (i < tileSize || i % tileSize == 0 || i > tileSize * tileSize - tileSize || (i + 1) % tileSize == 0)
                {
                    data[i] = Color.White;
                }
                else
                {
                    data[i] = color;
                }
            }
            tile.SetData(data);
            return tile;
        }

        private void TestDrawBackground(SpriteBatch spriteBatch)
        {
            int tileSize = 200;
            Vector2 startLocation = Vector2.Zero;

            int numberOfTilesX = (int)Math.Ceiling((decimal)viewport.Bounds.Width / tileSize);
            int numberOfTilesY = (int)Math.Ceiling((decimal)viewport.Bounds.Height / tileSize);
            playerTileX = (int)Math.Floor((player.Position.X) / tileSize);
            playerTileY = (int)Math.Floor((player.Position.Y) / tileSize);

            spriteBatch.Draw(testTile1, startLocation, Color.White);
            startLocation.X = startLocation.X- tileSize;
            DrawTile(-1, 0);

            int minX = (int)Math.Floor(playerTileX - (double)numberOfTilesX / 2);
            int maxX = (int)Math.Ceiling(playerTileX + (double)numberOfTilesX / 2);
            int minY = (int)Math.Floor(playerTileY - (double)numberOfTilesY / 2);
            int maxY = (int)Math.Ceiling(playerTileY + (double)numberOfTilesY / 2);

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
            var position = new Vector2(testTile1.Bounds.Width * x, testTile1.Bounds.Height * y);
            spriteBatch.Draw(testTile1, position, Color.White);
            spriteBatch.DrawString(debugFont, $"{x},{y}", new Vector2(position.X + 5, position.Y + 5), Color.White);
        }
    }
}
