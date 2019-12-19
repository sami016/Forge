using Forge.Core.Components;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Forge.Core.Engine
{
    public class Entity
    {
        private readonly IList<IComponent> _components = new List<IComponent>();
        private readonly IDictionary<uint, Entity> _children = new Dictionary<uint, Entity>();

        public IEnumerable<IComponent> Components => _components;

        /// <summary>
        /// The unit's unique identifier.
        /// </summary>
        public uint Id { get; set; }
        public Entity Parent { get; set; }
        public IEnumerable<Entity> Children => _children?.Values ?? new Entity[0];
        public IEnumerable<Entity> All => Children;
        public EntityManager EntityManager { get; private set; }
        public bool Deleted { get; private set; } = false;
        public bool Spawned { get; private set; } = false;

        internal Entity(EntityManager entityManager)
        {
            EntityManager = entityManager;
            Id = EntityManager.GenerateId();
        }

        public Entity(Entity parent, EntityManager entityManager) : this(entityManager)
        {
            Parent = parent;
        }

        public void Spawn()
        {
            if (Spawned)
            {
                throw new InvalidOperationException("Entity has already been spawned");
            }
            Spawned = true;
            EntityManager.Spawned(this);
        }

        //public void UpdateGeneric(Action<object> update)
        //{
        //    _updateQueue.EnqueueUpdate(update);
        //}

        ///// <summary>
        ///// Applies all updates in the queue of actions.
        ///// </summary>
        //public void ApplyUpdates()
        //{
        //    _updateQueue.Execute(this);
        //}

        /// <summary>
        /// Creates a child entity.
        /// </summary>
        /// <returns>child entity</returns>
        public Entity Create()
        {
            var entity = EntityManager.Create();
            entity.Parent = this;
            _children[entity.Id] = entity;

            return entity;
        }

        /// <summary>
        /// Creates a singleton child entity.
        /// The entity will be spawned automatically immediately.
        /// </summary>
        /// <returns></returns>
        public T Create<T>(T singletonComponent)
            where T : IComponent
        {
            var entity = Create();
            entity.Add<T>(singletonComponent);
            entity.Spawn();
            return singletonComponent;
        }

        public bool Has<T>()
        {
            var type = typeof(T);
            foreach (var component in _components)
            {
                if (type.IsAssignableFrom(component.GetType()))
                {
                    return true;
                }
            }
            return false;
        }

        public bool Has(Type type) 
        {
            foreach (var component in _components)
            {
                if (type.IsAssignableFrom(component.GetType()))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Creates a child entity using a template.
        /// </summary>
        /// <param name="template"></param>
        /// <returns></returns>
        //public Entity Create(IEntityTemplate template)
        //{
        //    var entity = Create();
        //    template.Create(entity);
        //    return entity;
        //}

        /// <summary>
        /// Deletes this entity.
        /// </summary>
        public void Delete()
        {
            Deleted = true;
            EntityManager.Despawn(this);
            foreach (var child in Children)
            {
                child.Delete();
            }
            foreach (var component in Components)
            {
                component.Dispose();
            }
        }

        public T Add<T>(T component)
            where T : IComponent
        {
            component.Entity = this;
            EntityManager.InitialiseComponent(component);
            if (!_components.Contains(component))
            {
                _components.Add(component);
            }
            return component;
        }

        public void Remove<T>(T component)
            where T : IComponent
        {
            if (_components.Contains(component))
            {
                _components.Remove(component);
            }
            component.Dispose();
        }

        public T Get<T>()
        {
            var type = typeof(T);
            foreach (var component in _components)
            {
                if (type.IsAssignableFrom(component.GetType()))
                {
                    return (T)component;
                }
            }
            return default(T);
        }

        public object Get(Type type)
        {
            foreach (var component in _components)
            {
                if (type.IsAssignableFrom(component.GetType()))
                {
                    return component;
                }
            }
            return null;
        }


        /// <summary>
        /// Gets all components of a given type.
        /// </summary>
        public IEnumerable<T> GetAll<T>() => GetAll(typeof(T)).Cast<T>();

        /// <summary>
        /// Gets all components of a given type.
        /// </summary>
        /// <param name="componentType">component type</param>
        /// <returns></returns>
        public IEnumerable<object> GetAll(Type componentType)
        {
            return _components
                .Where(x => componentType.IsAssignableFrom(x.GetType()));
        }
    }
}
