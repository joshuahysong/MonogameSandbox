using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StellarOps.Contracts;
using StellarOps.Projectiles;
using StellarOps.Weapons;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StellarOps.Ships
{
    public abstract class ShipBase : Entity, IContainer, IFocusable
    {
        public Vector2 WorldPosition { get; set; }
        public Vector2 Size { get; set; }
        public Vector2 Center => Size == null ? Vector2.Zero : new Vector2(Size.X / 2, Size.Y / 2);
        public List<Tile> Tiles { get; set; }
        public List<IPawn> Pawns { get; set; }
        public bool IsManeuvering { get; set; }
        public bool IsMainThrustFiring { get; set; }
        public bool IsPortThrustFiring { get; set; }
        public bool IsStarboardThrustFiring { get; set; }

        protected int[,] TileMapArtData;
        protected int[,] TileMapCollisionData;
        protected int[,] TileMapHealthData;
        protected float Thrust;
        protected float ManeuveringThrust;
        protected float MaxTurnRate;
        protected float MaxVelocity;
        protected List<WeaponBase> Weapons;

        private Vector2 _acceleration;
        private float _currentTurnRate;

        public ShipBase()
        {
            Pawns = new List<IPawn>();
            Weapons = new List<WeaponBase>();
        }

        public override void Update(GameTime gameTime, Matrix parentTransform)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            HandleInput(deltaTime);
            DetectCollisions();

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

            if (!IsManeuvering)
            {
                if (_currentTurnRate != 0)
                {
                    SlowDownManueveringThrust();
                }
            }

            Pawns.ForEach(p => p.Update(gameTime, LocalTransform));

            if (MainGame.IsDebugging && MainGame.Ship == this)
            {
                MainGame.Instance.ShipDebugEntries["Position"] = $"{Math.Round(Position.X)}, {Math.Round(Position.Y)}";
                MainGame.Instance.ShipDebugEntries["Velocity"] = $"{Math.Round(Velocity.X)}, {Math.Round(Velocity.Y)}";
                MainGame.Instance.ShipDebugEntries["Heading"] = $"{Math.Round(Heading, 2)}";
                MainGame.Instance.ShipDebugEntries["Velocity Heading"] = $"{Math.Round(Velocity.ToAngle(), 2)}";
                MainGame.Instance.ShipDebugEntries["World Tile"] = $"{Math.Floor(Position.X / MainGame.WorldTileSize)}, {Math.Floor(Position.Y / MainGame.WorldTileSize)}";
                MainGame.Instance.ShipDebugEntries["Current Turn Rate"] = $"{Math.Round(_currentTurnRate, 2)}";
            }
            Console.WriteLine(Size);
        }

        public void HandleInput(float deltaTime)
        {
            if (MainGame.Camera.Focus == this)
            {
                // Apply thrust
                if (Input.IsKeyPressed(Keys.W) || Input.IsKeyPressed(Keys.Up))
                {
                    ApplyForwardThrust();
                }
                // Rotate Counter-Clockwise
                if (Input.IsKeyPressed(Keys.A) || Input.IsKeyPressed(Keys.Left))
                {
                    ApplyPortManeuveringThrusters();
                }
                // Rotate Clockwise
                else if (Input.IsKeyPressed(Keys.D) || Input.IsKeyPressed(Keys.Right))
                {
                    ApplyStarboardManeuveringThrusters();
                }
                // Rotate to face retro thurst heading
                else if (Input.IsKeyPressed(Keys.S) || Input.IsKeyPressed(Keys.Down) || Input.IsKeyPressed(Keys.X))
                {
                    RotateToRetro(deltaTime, Input.IsKeyPressed(Keys.X));
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
                if (Input.IsKeyPressed(Keys.Space))
                {
                    FireWeapons();
                }

                // Tile click
                // TODO TEMP FOR DAMAGE TESTING
                if (Input.WasLeftMouseButtonClicked())
                {
                    Maybe<Tile> targetTile = GetTileByWorldPosition(Input.WorldMousePosition);
                    if (targetTile.HasValue)
                    {
                        if (targetTile.Value.Health > 0)
                        {
                            targetTile.Value.Health = targetTile.Value.Health - 25 < 0 ? 0 : targetTile.Value.Health - 25;
                            if (targetTile.Value.Health <= 0)
                            {
                                targetTile.Value.Health = 0;
                                targetTile.Value.CollisionType = CollisionType.None;
                            }
                        }
                    }
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch, Matrix parentTransform)
        {
            Matrix globalTransform = LocalTransform * parentTransform;

            Tiles.ForEach(t => t.Draw(spriteBatch, globalTransform));
            Pawns.ForEach(p => p.Draw(spriteBatch, globalTransform));

            IsManeuvering = false;
            IsMainThrustFiring = false;
            IsPortThrustFiring = false;
            IsStarboardThrustFiring = false;
        }

        public void ApplyForwardThrust()
        {
            _acceleration.X += Thrust * (float)Math.Cos(Heading);
            _acceleration.Y += Thrust * (float)Math.Sin(Heading);
            IsMainThrustFiring = true;
        }

        public void ApplyStarboardManeuveringThrusters()
        {
            _currentTurnRate = _currentTurnRate + ManeuveringThrust > MaxTurnRate ? MaxTurnRate
                : _currentTurnRate + ManeuveringThrust;
            IsManeuvering = true;
            IsStarboardThrustFiring = true;
        }

        public void ApplyPortManeuveringThrusters()
        {
            _currentTurnRate = _currentTurnRate - ManeuveringThrust < -MaxTurnRate ? -MaxTurnRate
                : _currentTurnRate - ManeuveringThrust;
            IsManeuvering = true;
            IsPortThrustFiring = true;
        }

        public void FireWeapons()
        {
            Weapons.ForEach(weapon => weapon.Fire(Heading, Velocity, Position));
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
            IsManeuvering = true;
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
                        IsPortThrustFiring = true;
                    }
                    else
                    {
                        _currentTurnRate = _currentTurnRate + ManeuveringThrust > MaxTurnRate ? MaxTurnRate
                            : _currentTurnRate + ManeuveringThrust;
                        IsStarboardThrustFiring = true;
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

        public Maybe<Tile> GetTileByWorldPosition(Vector2 position)
        {
            return GetTileByRelativePosition(Vector2.Transform(position, Matrix.Invert(LocalTransform)));
        }

        /// <summary>
        /// Gets the tile object at the given position
        /// </summary>
        /// <param name="position">Relative Position to check</param>
        /// <returns>Tile at position</returns>
        public Maybe<Tile> GetTileByRelativePosition(Vector2 position)
        {
            Vector2 relativePosition = position + Center;
            return Tiles.Any(t => t.Bounds.Contains(position))
                ? Maybe<Tile>.Some(Tiles.First(t => t.Bounds.Contains(position)))
                : Maybe<Tile>.None;
        }

        public Maybe<Tile> GetTileByPoint(Point location)
        {
            return Tiles.Any(t => t.Location == location)
                ? Maybe<Tile>.Some(Tiles.First(t => t.Location == location))
                : Maybe<Tile>.None;
        }

        protected void SwitchControlToShip()
        {
            MainGame.Camera.Focus = this;
            if (MainGame.Camera.Scale > 1f)
            {
                MainGame.Camera.Scale = 1F;
            }
        }

        protected void SetTiles()
        {
            Tiles = new List<Tile>();
            int? rows = TileMapArtData.GetLength(0);
            int? columns = TileMapArtData.GetLength(1);
            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x < columns; x++)
                {
                    Tile tile = new Tile
                    {
                        Container = this,
                        Location = new Point(x, y),
                        CollisionType = (CollisionType)TileMapCollisionData[y, x],
                        TileType = (TileType)TileMapArtData[y, x],
                        Health = TileMapHealthData[y, x],
                        North = y == 0 ? null : columns * y - columns + x,
                        NorthEast = y == 0 || x == columns - 1 ? null : columns * y - columns + x + 1,
                        East = x == columns - 1 ? null : (int?)Tiles.Count() + 1,
                        SouthEast = y == rows - 1 || x == columns - 1 ? null : columns * y + columns + x + 1,
                        South = y == rows - 1 ? null : columns * y + columns + x,
                        SouthWest = y == rows - 1 || x == 0 ? null : columns * y + columns + x - 1,
                        West = x == 0 ? null : (int?)Tiles.Count() - 1,
                        NorthWest = y == 0 || x == 0 ? null : columns * y - columns + x - 1,
                    };
                    tile.Position = new Vector2(x * MainGame.TileSize, y * MainGame.TileSize) - Center + tile.TileCenter;
                    Vector2 relativePosition = new Vector2(x * MainGame.TileSize, y * MainGame.TileSize);
                    relativePosition -= Center;
                    tile.Bounds = new Rectangle((int)relativePosition.X, (int)relativePosition.Y, MainGame.TileSize, MainGame.TileSize);
                    Tiles.Add(tile);
                }
            }

            Tiles.ForEach(tile =>
            {
                Tuple<Texture2D, Rectangle, float> tileData = TileHelper.GetTileData(tile, this);
                if (tileData != null)
                {
                    tile.Image = tileData.Item1;
                    tile.ImageSource = tileData.Item2;
                    tile.Heading = tileData.Item3;
                }
            });
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

        private void DetectCollisions()
        {
            float Radius = Size.X > Size.Y ? Size.X : Size.Y;
            List<ProjectileBase> projectiles = EntityManager.Projectiles.Select(e => e).ToList();
            for (var i = 0; i < projectiles.Count(); i++)
            {
                if ((projectiles[i].Position - WorldPosition).Length() <= Radius + projectiles[i].Radius)
                {
                    Maybe<Tile> tile = GetTileByWorldPosition(projectiles[i].Position);
                    if (tile.HasValue
                        && (tile.Value.CollisionType == CollisionType.All || tile.Value.CollisionType == CollisionType.Projectile))
                    {
                        tile.Value.Health -= 25;
                        if (tile.Value.Health <= 0)
                        {
                            tile.Value.Health = 0;
                            tile.Value.TileType = TileType.Empty;
                            tile.Value.CollisionType = CollisionType.None;
                        }
                        projectiles[i].IsExpired = true;
                    }
                }
            }
        }
    }
}
