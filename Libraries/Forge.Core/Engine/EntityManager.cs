using Forge.Core.Components;
using Forge.Core.Utilities;
using System;
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
        //private readonly ComponentInjector _injector;
        public SideEffectManager UpdateContext { get; set; }

        public EntityPool Pools { get; }
        public IEnumerable<Entity> All => Pools.Entities;

        public Entity Get(uint id) => Pools.Get(id);

        public EntityManager(EntityPool pools, IList<Type> indexTypes, SideEffectManager updateContext, IServiceProvider serviceProvider)
        {
            Pools = pools;
            UpdateContext = updateContext;
            ServiceProvider = serviceProvider;
            
            _componentIndexer = new ComponentIndexer(indexTypes);
            Pools.ItemAdded += ItemAdded;
        }

        public Entity Create()
        {
            var entity = new Entity(this);
            return entity;
        }

        //public Entity Create(IEntityTemplate template)
        //{
        //    var entity = Create();
        //    template.Create(entity);
        //    return entity;
        //}

        public void Despawn(uint id)
        {
            Pools.Remove(id);
        }

        private uint _idCurrent = 0;
        public uint GenerateId()
        {
            return _idCurrent++;
        }

        internal void Spawned(Entity entity)
        {
            entity.Update(() =>
            {
                // On the next time step, add component for indexing.
                // Useless indexing now, since it doesn't have components added until after this returns.
                _componentIndexer.Index(entity);
                entity.Id = Pools.Add(entity);
            });
        }

        public IEnumerable<T> GetAll<T>()
            where T : IComponent
        {
            var entities = _componentIndexer.GetAll<T>();
            if (entities == null)
            {
                // Unindexed search.
                entities = Pools.Entities
                    .Where(x => x != null && x.Has<T>());
            }
            return entities
                .SelectMany(x => x.GetAll<T>());
        }

        internal void Despawned(Entity entity)
        {
            Pools.Remove(entity.Id);
            // Remove all indexes for this entity immediately. We don't want this showing up when searching.
            _componentIndexer.Unindex(entity);
        }

        public Entity[] GetShardSet(int shardNumber)
        {
            return Pools.Sets[shardNumber];
        }

        /// <summary>
        /// This updates the thread context number on an entity whenever it is created.
        /// </summary>
        /// <param name="entity">entity</param>
        /// <param name="shardIndex">the index of the sharded pool this entity is in.</param>
        private void ItemAdded(Entity entity, int shardIndex)
        {
            entity.ThreadContextNumber = shardIndex;
        }

        public void InitialiseComponent(IComponent component)
        {
            ServiceProvider.Inject(component);
        }
    }
}
