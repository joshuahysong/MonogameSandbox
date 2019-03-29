using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StellarOps
{
    public abstract class Entity
    {
        public Vector2 Position { get; protected set; }
        public bool IsExpired { get; set; }
        public float Heading { get; set; }

        public Matrix LocalTransform =>
            Matrix.CreateTranslation(0, 0, 0f) *
            Matrix.CreateScale(1f, 1f, 1f) *
            Matrix.CreateRotationZ(Heading) *
            Matrix.CreateTranslation(Position.X, Position.Y, 0f);

        protected Vector2 Velocity;

        protected Color color = Color.White;

        public abstract void Update(GameTime gameTime, Matrix parentTransform);

        public abstract void Draw(SpriteBatch spriteBatch, Matrix parentTransform);

        public void DecomposeMatrix(ref Matrix matrix, out Vector2 position, out float rotation, out Vector2 scale)
        {
            matrix.Decompose(out Vector3 scale3, out Quaternion rotationQ, out Vector3 position3);
            Vector2 direction = Vector2.Transform(Vector2.UnitX, rotationQ);
            rotation = direction.ToAngle();
            position = new Vector2(position3.X, position3.Y);
            scale = new Vector2(scale3.X, scale3.Y);
        }
    }
}
