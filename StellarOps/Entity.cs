using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StellarOps.Ships;
using System;
using System.Collections.Generic;

namespace StellarOps
{
    public abstract class Entity
    {
        public Vector2 Position { get; set; }
        public Vector2 Velocity;
        public float Heading;
        public Texture2D Image;

        protected Color color = Color.White;
        protected Vector2 parentTile;

        public Vector2 ImageCenter => new Vector2(Image.Width / 2, Image.Height / 2);
        public Matrix LocalTransform {
            get {
                return Matrix.CreateTranslation(0, 0, 0f) *
                    Matrix.CreateScale(1f, 1f, 1f) *
                    Matrix.CreateRotationZ(Heading) *
                    Matrix.CreateTranslation(Position.X, Position.Y, 0f);
            }
        }
        public bool IsChild => Parent != null;

        public Entity Parent;
        public List<Entity> Children;

        public float Radius;
        public bool IsExpired;

        public Vector2 Size
        {
            get
            {
                return Image == null ? Vector2.Zero : new Vector2(Image.Width, Image.Height);
            }
        }

        public Entity()
        {
            Children = new List<Entity>();
        }

        public abstract void Update(GameTime gameTime, Matrix parentTransform);

        public virtual void Draw(SpriteBatch spriteBatch, Matrix parentTransform)
        {
            spriteBatch.Draw(Image, Position, null, color, Heading, Size / 2f, 1f, 0, 0);
        }

        public void DecomposeMatrix(ref Matrix matrix, out Vector2 position, out float rotation, out Vector2 scale)
        {
            Vector3 position3, scale3;
            Quaternion rotationQ;
            matrix.Decompose(out scale3, out rotationQ, out position3);
            Vector2 direction = Vector2.Transform(Vector2.UnitX, rotationQ);
            rotation = (float)Math.Atan2(direction.Y, direction.X);
            position = new Vector2(position3.X, position3.Y);
            scale = new Vector2(scale3.X, scale3.Y);
        }

        public Rectangle GetBoundingRectangle()
        {
            Vector2 rectangePosition = Position;
            if (IsChild)
            {
                Vector2 imageCenter = new Vector2(Parent.Image.Width / 2, Parent.Image.Height / 2);
                rectangePosition = Position + imageCenter;
            }

            int left = (int)Math.Round(rectangePosition.X);
            int top = (int)Math.Round(rectangePosition.Y);

            return new Rectangle(left, top, Image.Width, Image.Height);
        }
    }
}
