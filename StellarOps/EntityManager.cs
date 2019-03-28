using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StellarOps.Projectiles;
using System.Collections.Generic;
using System.Linq;

namespace StellarOps
{
    public static class EntityManager
    {
        public static List<Entity> Entities = new List<Entity>();
        public static List<ProjectileBase> Projectiles = new List<ProjectileBase>();
        private static List<Entity> addedEntities = new List<Entity>();
        private static bool isUpdating;

        public static int Count { get { return Entities.Count; } }

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
            if (entity is ProjectileBase)
            {
                Projectiles.Add((ProjectileBase)entity);
            }
            else
            {
                Entities.Add(entity);
            }
        }

        public static void Update(GameTime gameTime, Matrix parentTransform)
        {
            isUpdating = true;

            foreach (Entity entity in Entities)
            {
                entity.Update(gameTime, parentTransform);
            }
            foreach (ProjectileBase projectile in Projectiles)
            {
                projectile.Update(gameTime, parentTransform);
            }

            isUpdating = false;

            foreach (Entity entity in addedEntities)
            {
                AddEntity(entity);
            }

            addedEntities.Clear();

            // remove any expired entities.
            Entities = Entities.Where(x => !x.IsExpired).ToList();
            Projectiles = Projectiles.Where(x => !x.IsExpired).ToList();
        }

        public static void Draw(SpriteBatch spriteBatch, Matrix parentTransform)
        {
            foreach (Entity entity in Entities)
            {
                entity.Draw(spriteBatch, parentTransform);
            }
            foreach (ProjectileBase projectile in Projectiles)
            {
                projectile.Draw(spriteBatch, parentTransform);
            }
        }
    }
}
