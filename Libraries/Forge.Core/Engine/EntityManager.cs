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

            Pools.ItemAdded += ItemAdded;
            //ServiceContainer = new ServiceContainer();
            //ServiceContainer.Add(this);
            //_injector = new ComponentInjector(ServiceContainer);
        }

        public Entity Create()
        {
            var entity = new Entity(this);
            entity.Id = Pools.Add(entity);
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

        internal void Despawned(Entity entity)
        {
            Pools.Remove(entity.Id);
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
