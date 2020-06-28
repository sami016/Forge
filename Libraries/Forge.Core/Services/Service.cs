using Forge.Core.Components;
using Forge.Core.Engine;
using Forge.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Text;

namespace Forge.Core.Services
{
    /// <summary>
    /// A service is a set of resources running in the background.
    /// </summary>
    public class Service : Component, IDisposable, IInit
    {
        // List of all singleton entities within the service.
        private IDictionary<Type, Entity> _singletonEntities = new Dictionary<Type, Entity>();

        [Inject] public ServiceContainer ServiceContainer { get; set; }

        public virtual void Initialise()
        {
        }

        /// <summary>
        /// Creates a singleton entity within the service.
        /// This entity will automatically be deleted when the service is destroyed.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="singleton"></param>
        /// <returns></returns>
        public T CreateSingleton<T>(T singleton)
            where T : IComponent
        {
            ServiceContainer.AddService(typeof(T), singleton);
            _singletonEntities[typeof(T)] = singleton.Entity;
            return Entity.SpawnSingleton(singleton);
        }

        /// <summary>
        /// Creates a service scoped entity.
        /// When the service is unloaded, this entity will be disposed and deleted.
        /// </summary>
        /// <returns></returns>
        public Entity Create(bool inScene = true)
        {
            return Entity.Create();
        }

        /// <summary>
        /// Creates an entity within the service.
        /// When the service is unloaded, this entity will be disposed and deleted.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="component"></param>
        /// <returns></returns>
        public T Create<T>(T component)
            where T : IComponent
        {
            return Entity.SpawnSingleton(component);
        }

        public override void Dispose()
        {
            foreach (var type in _singletonEntities.Keys)
            {
                var entity = _singletonEntities[type];
                entity?.Delete();
                ServiceContainer.RemoveService(type);
            }
        }
    }
}
