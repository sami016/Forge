using Forge.Core.Components;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Forge.Core.Resources
{
    public class ResourceManager<T> : Component
    {
        public IDictionary<string, T> _loaded;

        public T Get(string key)
        {
            return _loaded.ContainsKey(key)
                ? _loaded[key]
                : default(T);
        }

        public void Load(string key, T value)
        {
            if (!_loaded.ContainsKey(key))
            {
                _loaded.Add(key, value);
            }
        }


        public void Unload(string key)
        {
            if (_loaded.ContainsKey(key))
            {
                _loaded.Remove(key);
            }
        }

        public void Unload(T value)
        {
            var entry = _loaded.Where(x => x.Value.Equals(value));
            if (entry.Any())
            {
                var first = entry.First();
                _loaded.Remove(first.Key);
            }
        }
    }
}
