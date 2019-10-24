using Forge.Core.Components;
using System;
using System.Collections.Generic;
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

        public ParallelCollection<Entity> Pools { get; }
        public IEnumerable<Entity> All => Pools.Entities;

        public Entity Get(uint id) => Pools.Get(id);

        public EntityManager(ParallelCollection<Entity> pools, SideEffectManager updateContext)
        {
            Pools = pools;
            UpdateContext = updateContext;

            _componentIndexer = new ComponentIndexer();
            Pools.ItemAdded += ItemAdded;
            //ServiceContainer = new ServiceContainer();
            //ServiceContainer.Add(this);
            //_injector = new ComponentInjector(ServiceContainer);
        }

        public Entity Create()
        {
            var entity = new Entity(this);
            entity.Id = Pools.Add(entity);
            // On the next time step, add component for indexing.
            // Useless indexing now, since it doesn't have components added until after this returns.
            this.Update(() => _componentIndexer.Index(entity));
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
            entity.Id = Pools.Add(entity);
        }

        public IEnumerable<T> GetAll<T>()
        {
            //TODO
            return new T[0];
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
    }
}
