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
        Texture2D playerShip;

        Vector2 velocity;
        Vector2 position;
        float rotation;
        int speed;

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
            // TODO: Add your initialization logic here

            position = new Vector2(GraphicsDevice.Viewport.Bounds.Width / 2, GraphicsDevice.Viewport.Bounds.Height / 2);
            speed = 0;
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

            playerShip = Content.Load<Texture2D>("ship");
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            //float velocity = 5f;

            //double angle = 0;

            //Vector2 trajectory = new Vector2(velocity) * new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));

            //if (Keyboard.GetState().IsKeyDown(Keys.W))
            //{
            //    position += trajectory;
            //}

            //Vector2 direction = new Vector2((float)Math.Cos(rotationAngle), (float)Math.Sin(rotationAngle));
            //direction.Normalize();
            //position += direction * 2;

            // Reset the velocity to zero after each update to prevent unwanted behavior
            velocity = Vector2.Zero;

            // Handle user input
            HandleInput(Keyboard.GetState(), gameTime);

            if (position.X <= 0)
            {
                position.X = GraphicsDevice.Viewport.Bounds.Width;
            }

            if (position.X > GraphicsDevice.Viewport.Bounds.Width)
            {
                position.X = 0;
            }

            if (position.Y <= 0)
            {
                position.Y = GraphicsDevice.Viewport.Bounds.Height;
            }

            if (position.Y > GraphicsDevice.Viewport.Bounds.Height)
            {
                position.Y = 0;
            }

            Console.WriteLine(speed);

            // Applies our speed to velocity
            //velocity *= speed;

            //velocity.X = (float)Math.Cos(rotation * 2 * Math.PI / 360);
            //velocity.Y = (float)-Math.Sin(rotation * 2 * Math.PI / 360);

            velocity = new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation));
            velocity.Normalize();
            // Seconds passed since iteration of update
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Multiplies our movement framerate independent by multiplying with deltaTime
            position += (velocity * deltaTime) * speed;

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            var textureCenter = new Vector2(playerShip.Width / 2, playerShip.Height / 2);

            spriteBatch.Begin();
            spriteBatch.Draw(playerShip, position, null, Color.White, rotation + (90 * (float)Math.PI / 180), textureCenter, 0.5f, SpriteEffects.None, 1f);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void HandleInput(KeyboardState KeyState, GameTime gameTime)
        {
            if (KeyState.IsKeyDown(Keys.W))
            {
                //Speed up
                speed += 1;
                if (speed > 200)
                {
                    speed = 200;
                }
            }
            if (KeyState.IsKeyDown(Keys.S))
            {
                speed -= 1;
                if (speed < 0)
                {
                    speed = 0;
                }
            }

            if (KeyState.IsKeyDown(Keys.A))
            {
                rotation -= 0.05F;
            }
            if (KeyState.IsKeyDown(Keys.D))
            {
                rotation += 0.05F;
            }
        }
    }
}
