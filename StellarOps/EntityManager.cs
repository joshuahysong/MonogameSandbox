using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace StellarOps
{
    public static class EntityManager
    {
        static List<Entity> entities = new List<Entity>();
        static List<Bullet> bullets = new List<Bullet>();

        static bool isUpdating;
        static List<Entity> addedEntities = new List<Entity>();

        public static int Count { get { return entities.Count; } }

        public static void Add(Entity entity)
        {
            if (!isUpdating)
            {
                AddEntity(entity);
            }
            else
            {
                addedEntities.Add(entity);
            }
        }

        private static void AddEntity(Entity entity)
        {
            entities.Add(entity);
            if (entity is Bullet)
            {
                bullets.Add(entity as Bullet);
            }
        }

        public static void Update(GameTime gameTime, Matrix parentTransform)
        {
            isUpdating = true;

            foreach (Entity entity in entities)
            {
                entity.Update(gameTime, parentTransform);
            }

            isUpdating = false;

            foreach (Entity entity in addedEntities)
            {
                entities.Add(entity);
            }

            addedEntities.Clear();

            // remove any expired entities.
            entities = entities.Where(x => !x.IsExpired).ToList();
            bullets = bullets.Where(x => !x.IsExpired).ToList();
        }

        public static void Draw(SpriteBatch spriteBatch, Matrix parentTransform)
        {
            foreach (Entity entity in entities.Where(e => !e.IsChild))
            {
                entity.Draw(spriteBatch, parentTransform);
            }
        }
    }
}
