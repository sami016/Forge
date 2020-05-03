using Forge.Core.Components;
using Forge.Core.Engine;
using Forge.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Text;

namespace Forge.Core.Scenes
{
    public abstract class Scene : Component, IDisposable, IInit
    {
        // List of all singleton entities within the scene.
        private IDictionary<Type, Entity> _singletonEntities = new Dictionary<Type, Entity>();
        [Inject] public ServiceContainer ServiceContainer { get; set; }

        protected Action Disposal;

        public abstract void Initialise();

        public void AddDisposeAction(Action disposeAction)
        {
            Disposal += disposeAction;
        }

        /// <summary>
        /// Creates a singleton entity within the scene.
        /// This entity will automatically be deleted when the scene is destroyed.
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
        /// Creates a scene scoped entity.
        /// When the scene is unloaded, this entity will be disposed and deleted.
        /// </summary>
        /// <returns></returns>
        public Entity Create(bool inScene = true)
        {
            return Entity.Create();
        }

        /// <summary>
        /// Creates an entity within the scene.
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
                ServiceContainer.RemoveService(type);
            }

            Disposal?.Invoke();
        }
    }
}
