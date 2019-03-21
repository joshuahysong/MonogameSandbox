using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StellarOps.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StellarOps.Ships
{
    public abstract class ShipCore : Entity, IContainer, IFocusable
    {
        public Vector2 WorldPosition { get; set; }
        public List<Tile> TileMap { get; set; }
        public List<IPawn> Pawns { get; set; }

        protected int[,] TileMapData;
        protected float Thrust;
        protected float ManeuveringThrust;
        protected float MaxTurnRate;
        protected float MaxVelocity;

        private Vector2 _acceleration;
        private float _currentTurnRate;

        private Dictionary<TileType, Texture2D> tileSprites;

        public ShipCore()
        {
            Pawns = new List<IPawn>();

            tileSprites = new Dictionary<TileType, Texture2D>
            {
                { TileType.Hull, Art.CreateRectangle(MainGame.TileSize, MainGame.TileSize, Color.DimGray, Color.Gray) },
                { TileType.Floor, Art.CreateRectangle(MainGame.TileSize, MainGame.TileSize, Color.White, Color.WhiteSmoke) },
                { TileType.FlightConsole, Art.CreateRectangle(MainGame.TileSize, MainGame.TileSize, Color.Khaki, Color.WhiteSmoke) },
            };
        }

        public override void Update(GameTime gameTime, Matrix parentTransform)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            HandleInput(deltaTime);

            // Continue rotation until turn rate reaches zero to simulate slowing
            if (_currentTurnRate > 0)
            {
                RotateClockwise(deltaTime);
            }
            else if (_currentTurnRate < 0)
            {
                RotateCounterClockwise(deltaTime);
            }

            Velocity += _acceleration * deltaTime;

            // Cap velocity to max velocity
            if (Velocity.LengthSquared() > MaxVelocity * MaxVelocity)
            {
                Velocity.Normalize();
                Velocity *= MaxVelocity;
            }
            _acceleration.X = 0;
            _acceleration.Y = 0;
            Position += Velocity * deltaTime;
            WorldPosition = Position;

            Pawns.ForEach(p => p.Update(gameTime, LocalTransform));

            if (MainGame.IsDebugging)
            {
                MainGame.Instance.ShipDebugEntries["Position"] = $"{Math.Round(Position.X)}, {Math.Round(Position.Y)}";
                MainGame.Instance.ShipDebugEntries["Velocity"] = $"{Math.Round(Velocity.X)}, {Math.Round(Velocity.Y)}";
                MainGame.Instance.ShipDebugEntries["Heading"] = $"{Math.Round(Heading, 2)}";
                MainGame.Instance.ShipDebugEntries["Velocity Heading"] = $"{Math.Round(Velocity.ToAngle(), 2)}";
                MainGame.Instance.ShipDebugEntries["World Tile"] = $"{Math.Floor(Position.X / MainGame.WorldTileSize)}, {Math.Floor(Position.Y / MainGame.WorldTileSize)}";
                MainGame.Instance.ShipDebugEntries["Current Turn Rate"] = $"{Math.Round(_currentTurnRate, 2)}";
            }
        }

        public void HandleInput(float deltaTime)
        {
            if (MainGame.Camera.Focus == this)
            {
                // Apply thrust
                if (Input.IsKeyPressed(Keys.W) || Input.IsKeyPressed(Keys.Up))
                {
                    _acceleration.X += Thrust * (float)Math.Cos(Heading);
                    _acceleration.Y += Thrust * (float)Math.Sin(Heading);
                }
                // Rotate Counter-Clockwise
                if (Input.IsKeyPressed(Keys.A) || Input.IsKeyPressed(Keys.Left))
                {
                    _currentTurnRate = _currentTurnRate - ManeuveringThrust < -MaxTurnRate ? -MaxTurnRate
                        : _currentTurnRate - ManeuveringThrust;
                }
                // Rotate Clockwise
                else if (Input.IsKeyPressed(Keys.D) || Input.IsKeyPressed(Keys.Right))
                {
                    _currentTurnRate = _currentTurnRate + ManeuveringThrust > MaxTurnRate ? MaxTurnRate
                        : _currentTurnRate + ManeuveringThrust;
                }
                // Rotate to face retro thurst heading
                else if (Input.IsKeyPressed(Keys.S) || Input.IsKeyPressed(Keys.Down))
                {
                    RotateToRetro(deltaTime, false);
                }
                // Rotate to face retro thurst heading and thrust to brake
                else if (Input.IsKeyPressed(Keys.X))
                {
                    RotateToRetro(deltaTime, true);
                }
                else
                {
                    if (_currentTurnRate != 0)
                    {
                        SlowDownManueveringThrust();
                    }
                }
                // Toggle to Player pawn control
                if (Input.IsKeyToggled(Keys.F) && !Input.ManagedKeys.Contains(Keys.F))
                {
                    Input.ManagedKeys.Add(Keys.F);
                    MainGame.Camera.Focus = MainGame.Player;
                    if (MainGame.Camera.Scale < 2f)
                    {
                        MainGame.Camera.Scale = 2F;
                    }
                }
            }
            else
            {
                if (_currentTurnRate != 0)
                {
                    SlowDownManueveringThrust();
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch, Matrix parentTransform)
        {
            // Calculate global transform
            Matrix globalTransform = LocalTransform * parentTransform;

            // Get values from GlobalTransform for SpriteBatch and render sprite
            DecomposeMatrix(ref globalTransform, out Vector2 position, out float rotation, out Vector2 scale);
            spriteBatch.Draw(Image, position, null, Color.White, rotation, ImageCenter, scale, SpriteEffects.None, 0.0f);

            // Tiles
            Vector2 imageCenter = new Vector2(Image.Width / 2, Image.Height / 2);
            Vector2 origin = imageCenter;
            TileMap.Where(t => tileSprites.Keys.Contains(t.TileType)).ToList().ForEach(tile =>
            {
                Texture2D tileToDraw = tileSprites[tile.TileType];
                Vector2 offset = new Vector2(tile.Location.X * MainGame.TileSize, tile.Location.Y * MainGame.TileSize);
                origin = imageCenter - offset;
                spriteBatch.Draw(tileToDraw, Position, null, Color.White, Heading, origin, 1f, SpriteEffects.None, 1f);
            });

            Pawns.ForEach(p => p.Draw(spriteBatch, globalTransform));
        }

        private void RotateClockwise(float deltaTime)
        {
            Heading += _currentTurnRate * deltaTime;
            if (Heading > Math.PI)
            {
                Heading = (float)-Math.PI;
            }
        }

        private void RotateCounterClockwise(float deltaTime)
        {
            Heading += _currentTurnRate * deltaTime;
            if (Heading < -Math.PI)
            {
                Heading = (float)Math.PI;
            }
        }

        private void RotateToRetro(float deltaTime, bool IsBraking)
        {
            float movementHeading = Velocity.ToAngle();
            float retroHeading = movementHeading < 0 ? movementHeading + (float)Math.PI : movementHeading - (float)Math.PI;
            if (Heading != retroHeading  && !IsWithinBrakingRange())
            {
                double retroDegrees = (retroHeading + Math.PI) * (180.0 / Math.PI);
                double headingDegrees = (Heading + Math.PI) * (180.0 / Math.PI);
                double turnRateDegrees = Math.PI * 2 * (_currentTurnRate * deltaTime) / 100 * 360 * 2;
                turnRateDegrees = turnRateDegrees < 0 ? turnRateDegrees * -1 : turnRateDegrees;
                double retroOffset = headingDegrees < retroDegrees ? (headingDegrees + 360) - retroDegrees : headingDegrees - retroDegrees;

                double thrustMagnitude = Math.Round(_currentTurnRate / ManeuveringThrust * _currentTurnRate);
                thrustMagnitude = thrustMagnitude < 0 ? thrustMagnitude * -1 : thrustMagnitude;

                if (retroOffset >= 360 - turnRateDegrees || retroOffset <= turnRateDegrees)
                {
                    Heading = retroHeading;
                    _currentTurnRate = 0;
                }
                else if (retroOffset > thrustMagnitude && 360 - retroOffset > thrustMagnitude)
                {
                    if (retroOffset < 180)
                    {
                        _currentTurnRate = _currentTurnRate - ManeuveringThrust < -MaxTurnRate ? -MaxTurnRate
                            : _currentTurnRate - ManeuveringThrust;
                    }
                    else
                    {
                        _currentTurnRate = _currentTurnRate + ManeuveringThrust > MaxTurnRate ? MaxTurnRate
                            : _currentTurnRate + ManeuveringThrust;
                    }
                }
                else
                {
                    SlowDownManueveringThrust();
                }
            }
            else if (IsBraking)
            {
                if (IsWithinBrakingRange())
                {
                    Velocity = Vector2.Zero;
                }
                else if (Heading == retroHeading)
                {
                    _acceleration.X += Thrust * (float)Math.Cos(Heading);
                    _acceleration.Y += Thrust * (float)Math.Sin(Heading);
                }
            }
        }

        private bool IsWithinBrakingRange()
        {
            double brakingRange = MaxVelocity / 100;
            return Velocity.X < brakingRange && Velocity.X > -brakingRange && Velocity.Y < brakingRange && Velocity.Y > -brakingRange;
        }

        public abstract void UseTile(Vector2 position);

        public abstract string GetUsePrompt(Vector2 Position);

        /// <summary>
        /// Gets the tile object at the given position
        /// </summary>
        /// <param name="position">Position to check</param>
        /// <returns>Tile at position</returns>
        public Tile GetTile(Vector2 position)
        {
            Vector2 relativePosition = position + ImageCenter;
            int tileX = (int)Math.Floor(relativePosition.X / MainGame.TileSize);
            int tileY = (int)Math.Floor(relativePosition.Y / MainGame.TileSize);
            return TileMap.FirstOrDefault(t => t.Location == new Point(tileX, tileY));
        }

        public Tile GetTile(Point location)
        {
            return TileMap.FirstOrDefault(t => t.Location == location);
        }

        protected void SwitchControlToShip()
        {
            MainGame.Camera.Focus = this;
            if (MainGame.Camera.Scale > 1f)
            {
                MainGame.Camera.Scale = 1F;
            }
        }

        protected List<Tile> GetTileMap()
        {
            List<Tile> tileMap = new List<Tile>();
            for (int y = 0; y < TileMapData.GetLength(0); y++)
            {
                for (int x = 0; x < TileMapData.GetLength(1); x++)
                {
                    Tile tile = new Tile
                    {
                        Location = new Point(x, y),
                        Collidable = (TileType)TileMapData[y, x] == TileType.Hull,
                        TileType = (TileType)TileMapData[y, x]
                    };
                    Vector2 relativePosition = new Vector2(x * MainGame.TileSize, y * MainGame.TileSize);
                    relativePosition -= ImageCenter;
                    tile.Bounds = new Rectangle((int)relativePosition.X, (int)relativePosition.Y, MainGame.TileSize, MainGame.TileSize);
                    tileMap.Add(tile);
                }
            }
            return tileMap;
        }


        private void SlowDownManueveringThrust()
        {
            if (_currentTurnRate < 0)
            {
                _currentTurnRate = _currentTurnRate + ManeuveringThrust > 0 ? 0 : _currentTurnRate + ManeuveringThrust;
            }
            else if (_currentTurnRate > 0)
            {
                _currentTurnRate = _currentTurnRate - ManeuveringThrust < 0 ? 0 : _currentTurnRate - ManeuveringThrust;
            }
        }
    }
}
