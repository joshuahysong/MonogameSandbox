using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace TestFlying
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

        public Vector2 CenterScreen;

        Player player;
        Camera camera;

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
            spriteBatch.DrawString(debugFont, $"Player: {Math.Round(player.Position.X)}, {Math.Round(player.Position.Y)}", new Vector2(viewport.Bounds.X + 5, viewport.Bounds.Y + 5), Color.White);
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

        private void TestDrawBackground(SpriteBatch spriteBatch)
        {
            int tileSize = 200;
            Vector2 startLocation = new Vector2(-tileSize / 2, -tileSize / 2);

            testTile1 = new Texture2D(GraphicsDevice, tileSize, tileSize);
            Color[] data = new Color[tileSize * tileSize];
            for (int i = 0; i < data.Length; ++i) data[i] = Color.Brown;
            testTile1.SetData(data);

            testTile2 = new Texture2D(GraphicsDevice, tileSize, tileSize);
            for (int i = 0; i < data.Length; ++i) data[i] = Color.DarkCyan;
            testTile2.SetData(data);

            int numberOfTilesX = (int)Math.Ceiling((decimal)viewport.Bounds.Width / tileSize);
            int numberOfTilesY = (int)Math.Ceiling((decimal)viewport.Bounds.Height / tileSize);

            spriteBatch.Draw(testTile1, startLocation, Color.White);
            //spriteBatch.Draw(test, CenterScreen, null, Color.DarkCyan, 0f, new Vector2(0.5f, 0.5f), new Vector2(tileSize, tileSize), SpriteEffects.None, 0f);
        }
    }
}
