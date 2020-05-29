using Forge.Core.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Forge.Core.Components
{
    /// <summary>
    /// Indexes components for fast lookups.
    /// </summary>
    public class ComponentIndexer
    {
        private IList<Type> _indexTypes = new List<Type>();
        private IDictionary<Type, IList<Entity>> _indexes = new Dictionary<Type, IList<Entity>>();

        public ComponentIndexer(IList<Type> indexTypes)
        {
            foreach (var type in indexTypes)
            {
                AddIndex(type);
            }
        }

        public void AddIndex(Type type)
        {
            if (!_indexes.ContainsKey(type))
            {
                _indexTypes.Add(type);
                _indexes[type] = new List<Entity>();
            }
        }

        public void Index(Entity entity)
        {
            // For each component, check if there are any indexes we need to add.
            foreach (var component in entity.Components)
            {
                foreach (var indexType in _indexTypes)
                {
                    if (indexType.IsAssignableFrom(component.GetType()))
                    {
                        var index = _indexes[indexType];
                        // Never index twice for a given index.
                        lock (index)
                        {
                            if (!index.Contains(entity))
                            {
                                index.Add(entity);
                            }
                        }
                    }
                }
            }
        }

        public void Unindex(Entity entity)
        {
            foreach (var indexType in _indexTypes)
            {
                var index = _indexes[indexType];
                lock (index)
                {
                    if (index.Contains(entity))
                    {
                        index.Remove(entity);
                    }
                }
            }
        }

        public IEnumerable<Entity> GetAll<T>() => GetAll(typeof(T));

        public IEnumerable<Entity> GetAll(Type componentType)
        {
            if (_indexes.ContainsKey(componentType))
            {
                var index = _indexes[componentType];
                lock (index)
                {
                    return index.ToArray();
                }
            }
            return null;
        }
    }
}
