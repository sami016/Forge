using Forge.Core.Components;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Forge.Core.Engine
{
    public class EntityPool
    {
        /// <summary>
        /// Raised whenever an item is added, including information about the shard it is added to.
        /// </summary>
        public event Action<Entity, int> ItemAdded;

        private readonly EntityPoolShard[] _shards;
        private readonly uint _blockSize;

        public EntityPool(IList<Type> indexTypes) : this(Environment.ProcessorCount, ushort.MaxValue, indexTypes)
        {
        }

        public EntityPool(int shardCount, uint capacity, IList<Type> indexTypes)
        {
            _shards = new EntityPoolShard[shardCount];
            _blockSize = (uint)(capacity / shardCount);
            for (var i = 0; i < shardCount; i++)
            {
                var startIndex = (uint)(i * _blockSize);
                var pool = new EntityPoolShard((uint)i, (uint)_blockSize, (uint)startIndex, indexTypes);
                _shards[i] = pool;
            }
        }

        public uint Add(Entity item)
        {
            // Selects the current smallest shard.
            var smallest = _shards[0].Size;
            var smallestIndex = 0;
            for (var i = 1; i < _shards.Length; i++)
            {
                if (_shards[i].Size < smallest)
                {
                    smallest = _shards[i].Size;
                    smallestIndex = i;
                }
            }

            // Selects the next available id.
            var pool = _shards[smallestIndex];
            var id = pool.Add(item);
            ItemAdded?.Invoke(item, smallestIndex);

            return id;
        }

        public void Remove(uint id)
        {
            var sectionIndex = (id / _blockSize);
            _shards[sectionIndex].Release(id);
        }

        public Entity Get(uint id)
        {
            var sectionIndex = (id / _blockSize);
            return _shards[sectionIndex].Get(id);
        }

        public Entity[][] Sets
        {
            get
            {
                return _shards
                    .Select(x => x.Entries)
                    .ToArray();
            }
        }

        public EntityPoolShard[] Shards => _shards;

        public IEnumerable<Entity> Entities
        {
            get
            {
                return _shards.SelectMany(x => x.Entries)
                    .ToArray();
            }
        }

        public class EntityPoolShard
        {
            private object _lock = new object();
            private readonly ComponentIndexer _componentIndexer;

            public EntityPoolShard(uint index, uint capacity, uint startIndex, IList<Type> indexTypes)
            {
                _index = index;
                Capacity = capacity;
                StartIndex = startIndex;
                EndIndex = startIndex + capacity;
                Size = 0;
                Entries = new Entity[capacity];
                _componentIndexer = new ComponentIndexer(indexTypes);
                HeadPointer = 0;
            }

            // Existing entries.
            public Entity[] Entries;
            // Number of items in the pool record.
            public uint Size;
            private readonly uint _index;

            // Total capacity of the pool.
            public uint Capacity;
            // Inclusive lower bound.
            public uint StartIndex;
            // Non-inclusive upper bound.
            public uint EndIndex;
            // Head pointer. At first points to free spaces.
            public uint HeadPointer;

            private uint AllocateLocalIndex()
            {
                for (var i = HeadPointer; i < Capacity; i++)
                {
                    if (Entries[i] == null)
                    {
                        return i;
                    }
                }
                throw new Exception("Out of pool space");
            }

            private void AdvanceHead()
            {
                HeadPointer++;
                HeadPointer %= Capacity;
            }

            public uint Add(Entity item)
            {
                lock (_lock)
                {
                    var freeLocal = AllocateLocalIndex();
                    Entries[freeLocal] = item;
                    AdvanceHead();
                    Size++;

                    // Index components.
                    item.Update(() =>
                    {
                        // If the entity is deleted in the first tick it exists...
                        if (item.Deleted)
                        {
                            return;
                        }
                        if (item.Components.Any())
                        {
                            var first = item.Components.First();
                            var componentName = first.GetType().Name;
                            Debug.WriteLine($"[{_index}] Added singleton {componentName} - id {item.Id}");
                        } else
                        {
                            Debug.WriteLine($"[{_index}] Added entity - id {item.Id}");
                        }
                        _componentIndexer.Index(item);
                    });

                    return freeLocal + StartIndex;
                }
            }

            public void Release(uint id)
            {
                Console.WriteLine($"Entity pool release - pool: {_index} id: {id}");
                Entity deletedEntity = null;
                lock (_lock)
                {
                    var localId = id - StartIndex;
                    deletedEntity = Entries[localId];
                    Entries[localId] = default(Entity);
                    Size--;
                }
                if (deletedEntity != null)
                {
                    deletedEntity.EntityManager.Update(() => _componentIndexer.Unindex(deletedEntity));
                }
            }

            public Entity Get(uint id)
            {
                var localId = id - StartIndex;
                return Entries[localId];
            }

            public IEnumerable<T> GetAll<T>()
            {
                var entities = _componentIndexer.GetAll<T>();
                if (entities == null)
                {
                    // Unindexed search.
                    entities = Entries
                        .Where(x => x != null && x.Has<T>());
                }
                return entities
                    .SelectMany(x => x.GetAll<T>());
            }
        }
    }
}
