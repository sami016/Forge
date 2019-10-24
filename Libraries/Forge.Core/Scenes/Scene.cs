using Forge.Core.Components;
using Forge.Core.Engine;
using Forge.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Forge.Core.Scenes
{
    public abstract class Scene : IDisposable, IInit
    {
        public IDictionary<Type, Entity> _singletonEntities = new Dictionary<Type, Entity>();

        [Inject] public EntityManager EntityManager { get; set; }

        public abstract void Initialise();

        public T AddSingleton<T>(T singleton)
            where T : IComponent
        {
            var type = typeof(T);
            var entity = new Entity(EntityManager);
            entity.Add(singleton);
            _singletonEntities[type] = entity;

            return singleton;
        }

        public virtual void Dispose()
        {
            foreach (var type in _singletonEntities.Keys)
            {
                var entity = _singletonEntities[type];
                entity.Delete();
            }
        }
    }
}
