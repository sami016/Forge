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
        private readonly object _lock = new object();

        /// <summary>
        /// Raised whenever an item is added, including information about the shard it is added to.
        /// </summary>
        public event Action<Entity> ItemAdded;

        // Existing entries.
        public Entity[] Entries;
        // Total capacity of the pool.
        private uint Capacity;
        // Head pointer. At first points to free spaces.
        private uint HeadPointer;
        // Number of items in the pool record.
        public uint Size;

        public EntityPool() : this(ushort.MaxValue)
        {
        }

        public EntityPool(uint capacity)
        {
            Capacity = capacity;
            Entries = new Entity[capacity];
        }

        public uint Add(Entity item)
        {
            uint id;
            lock (_lock)
            {
                id = AllocateId();
                Entries[id] = item;
                AdvanceHead();
                Size++;

                return id;
            }

            ItemAdded?.Invoke(item);

            return id;
        }

        public void Remove(uint id)
        {
            Console.WriteLine($"Entity pool remove - id: {id}");
            lock (_lock)
            {
                Entries[id] = default(Entity);
                Size--;
            }
        }

        public Entity Get(uint id)
        {
            return Entries[id];
        }

        private void AdvanceHead()
        {
            HeadPointer++;
            HeadPointer %= Capacity;
        }

        public IEnumerable<Entity> Entities => Entries.Where(x => x != null);

        private uint AllocateId()
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

        // REMOVE

        //public class EntityPoolShard
        //{
        //    private object _lock = new object();
        //    private readonly ComponentIndexer _componentIndexer;

        //    public EntityPoolShard(uint index, uint capacity, uint startIndex, IList<Type> indexTypes)
        //    {
        //        _index = index;
        //        Capacity = capacity;
        //        StartIndex = startIndex;
        //        EndIndex = startIndex + capacity;
        //        Size = 0;
        //        Entries = new Entity[capacity];
        //        _componentIndexer = new ComponentIndexer(indexTypes);
        //        HeadPointer = 0;
        //    }

        //    private readonly uint _index;


        //    private uint AllocateLocalIndex()
        //    {
        //        for (var i = HeadPointer; i < Capacity; i++)
        //        {
        //            if (Entries[i] == null)
        //            {
        //                return i;
        //            }
        //        }
        //        throw new Exception("Out of pool space");
        //    }


        //    public uint Add(Entity item)
        //    {
        //        lock (_lock)
        //        {
        //            var freeLocal = AllocateLocalIndex();
        //            Entries[freeLocal] = item;
        //            AdvanceHead();
        //            Size++;

        //            // Index components.
        //            // If the entity is deleted in the first tick it exists...
        //            //if (item.Deleted)
        //            //{
        //            //    return;
        //            //}
        //            if (item.Components.Any())
        //            {
        //                var first = item.Components.First();
        //                var componentName = first.GetType().Name;
        //                Debug.WriteLine($"[{_index}] Added singleton {componentName} - id {item.Id}");
        //            } else
        //            {
        //                Debug.WriteLine($"[{_index}] Added entity - id {item.Id}");
        //            }
        //            _componentIndexer.Index(item);

        //            return freeLocal + StartIndex;
        //        }
        //    }

        //    public void Release(uint id)
        //    {
        //    }

        //    public Entity Get(uint id)
        //    {
        //        var localId = id - StartIndex;
        //        return Entries[localId];
        //    }

        //}
    }
}
