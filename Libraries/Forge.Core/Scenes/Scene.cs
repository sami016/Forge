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
        // List of all singleton entities within the scene.
        private IDictionary<Type, Entity> _singletonEntities = new Dictionary<Type, Entity>();
        // List of all non-signleton entities within the scene.
        private IList<Entity> _listEntities = new List<Entity>();

        [Inject] public EntityManager EntityManager { get; set; }

        protected Action Disposal;

        public abstract void Initialise();

        public T AddSingleton<T>(T singleton)
            where T : IComponent
        {
            var type = typeof(T);
            var entity = EntityManager.Create();
            entity.Add(singleton);
            _singletonEntities[type] = entity;

            return singleton;
        }

        /// <summary>
        /// Creates a scene scoped entity.
        /// When the scene is unloaded, this entity will be disposed and deleted.
        /// </summary>
        /// <returns></returns>
        public Entity Create()
        {
            var entity = EntityManager.Create();
            _listEntities.Add(entity);
            return entity;
        }

        public virtual void Dispose()
        {
            foreach (var type in _singletonEntities.Keys)
            {
                var entity = _singletonEntities[type];
                entity.Delete();
            }
            foreach (var entity in _listEntities)
            {
                entity.Delete();
            }

            Disposal?.Invoke();
        }
    }
}
