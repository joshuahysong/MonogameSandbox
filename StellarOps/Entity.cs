using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace StellarOps
{
    public abstract class Entity
    {
        protected Color color = Color.White;

        public Vector2 Position { get; set; }
        public Vector2 Velocity;
        public float Heading;
        public Texture2D Image;
        public Matrix LocalTransform {
            get {
                // Transform = -Origin * Scale * Rotation * Translation
                return Matrix.CreateTranslation(-Image.Width / 2f, -Image.Height / 2f, 0f) *
                       Matrix.CreateScale(1f, 1f, 1f) *
                       Matrix.CreateRotationZ(Heading) *
                       Matrix.CreateTranslation(Position.X, Position.Y, 0f);
            }
        }
        public List<Entity> Children;

        public float Radius = 20;
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

        public abstract void Update(GameTime gameTime);

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
    }
}
