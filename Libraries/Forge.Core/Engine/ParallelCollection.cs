using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Forge.Core.Engine
{
    public class ParallelCollection<T>
    {
        /// <summary>
        /// Raised whenever an item is added, including information about the shard it is added to.
        /// </summary>
        public event Action<T, int> ItemAdded;

        private readonly Pool[] _shards;
        private readonly uint _blockSize;

        public ParallelCollection() : this(Environment.ProcessorCount, ushort.MaxValue)
        {
        }

        public ParallelCollection(int shardCount, uint capacity)
        {
            _shards = new Pool[shardCount];
            _blockSize = (uint)(capacity / shardCount);
            for (var i = 0; i < shardCount; i++)
            {
                var startIndex = (uint)(i * _blockSize);
                var pool = new Pool((uint)_blockSize, (uint)startIndex);
                _shards[i] = pool;
            }
        }

        public uint Add(T item)
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

        public T Get(uint id)
        {
            var sectionIndex = (id / _blockSize);
            return _shards[sectionIndex].Get(id);
        }

        public T[][] Sets
        {
            get
            {
                return _shards
                    .Select(x => x.Entries)
                    .ToArray();
            }
        }

        public IEnumerable<T> Entities
        {
            get
            {
                return _shards.SelectMany(x => x.Entries)
                    .ToArray();
            }
        }

        private class Pool
        {
            private object _lock = new object();

            public Pool(uint capacity, uint startIndex)
            {
                Capacity = capacity;
                StartIndex = startIndex;
                EndIndex = startIndex + capacity;
                Size = 0;
                Entries = new T[capacity];
                HeadPointer = 0;
            }

            // Existing entries.
            public T[] Entries;
            // Number of items in the pool record.
            public uint Size;
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

            public uint Add(T item)
            {
                lock (_lock)
                {
                    var freeLocal = AllocateLocalIndex();
                    Entries[freeLocal] = item;
                    AdvanceHead();
                    Size++;
                    return freeLocal + StartIndex;
                }
            }

            public void Release(uint id)
            {
                lock (_lock)
                {
                    var localId = id - StartIndex;
                    Entries[localId] = default(T);
                    Size--;
                }
            }

            public T Get(uint id)
            {
                var localId = id - StartIndex;
                return Entries[localId];
            }
        }
    }
}
