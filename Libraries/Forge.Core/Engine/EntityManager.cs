using Forge.Core.Components;
using Forge.Core.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Forge.Core.Engine
{
    /// <summary>
    /// Single local source of truth for all entities in the game.
    /// </summary>
    public class EntityManager
    {
        private uint _autoGenCounter = 1;
        private ComponentIndexer _componentIndexer;

        public IServiceProvider ServiceProvider { get; }
        public EntityPool Pools { get; }
        public IEnumerable<Entity> All => Pools.Entities;

        public Entity Get(uint id) => Pools.Get(id);

        public EntityManager(EntityPool pools, IList<Type> indexTypes, IServiceProvider serviceProvider)
        {
            Pools = pools;
            ServiceProvider = serviceProvider;
            
            _componentIndexer = new ComponentIndexer(indexTypes);
        }

        public Entity Create()
        {
            var entity = new Entity(this);
            return entity;
        }

        public Entity Create<T>(T component)
            where T : IComponent
        {
            var entity = new Entity(this);
            entity.Add(component);
            entity.Spawn();
            return entity;
        }

        //public Entity Create(IEntityTemplate template)
        //{
        //    var entity = Create();
        //    template.Create(entity);
        //    return entity;
        //}

        public void Despawn(Entity entity)
        {
            Console.WriteLine($"Despawning entity with id: {entity.Id}");
            Pools.Remove(entity.Id);
            // Remove all indexes for this entity. We don't want this showing up when searching.
            _componentIndexer.Unindex(entity);
        }

        private uint _idCurrent = 0;
        public uint GenerateId()
        {
            return _idCurrent++;
        }

        internal void Spawned(Entity entity)
        {
            //// If the entity is deleted in the first tick it exists...
            //if (entity.Deleted)
            //{
            //    return;
            //}
            // On the next time step, add component for indexing.
            // Useless indexing now, since it doesn't have components added until after this returns.
            _componentIndexer.Index(entity);
            entity.Id = Pools.Add(entity);
            Console.WriteLine($"Spawned entity with id {entity.Id}");
            foreach (var component in entity.Components)
            {
                Console.WriteLine($"\twith component: {component.GetType()}");
            }
        }

        internal void Reindex(Entity entity)
        {
            // Reindex the entity.
            _componentIndexer.Index(entity);
        }

        public IEnumerable<T> GetAll<T>() => GetAll(typeof(T)).Cast<T>();

        public IEnumerable<object> GetAll(Type componentType)
        {
            var entities = _componentIndexer.GetAll(componentType);
            if (entities == null)
            {
                // Unindexed search.
                entities = Pools.Entities
                    .Where(x => x != null && x.Has(componentType));
            }
            return entities
                .ToArray()
                .SelectMany(x => x.GetAll(componentType));
        }

        public void InitialiseComponent(IComponent component)
        {
            ServiceProvider.Inject(component);
        }
    }
}
