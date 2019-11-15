using Forge.Core.Components;
using Forge.Core.Engine;
using Forge.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Forge.Core.Utilities
{
    public static class InjectionExtensions
    {
        public static void Inject(this IServiceProvider serviceProvider, object target)
        {
            InjectInternally(serviceProvider, target);
            if (target is IInit init)
            {
                init.Initialise();
            }
        }

        private static void InjectInternally(IServiceProvider serviceProvider, object target)
        {
            var component = target as IComponent;
            var type = target.GetType();
            foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(x => x.GetCustomAttribute<InjectAttribute>() != null))
            {
                if (!property.CanWrite)
                {
                    continue;
                }

                // If a component exists on the entity, inject this.
                if (component != null
                    && component.Entity.Has(property.PropertyType))
                {
                    property.SetValue(target, component.Entity.Get(property.PropertyType));
                    continue;
                }

                // If a service exists, inject this.
                var lookedUpService = serviceProvider.GetService(property.PropertyType);
                if (lookedUpService != null) {
                    property.SetValue(target, lookedUpService);
                    continue;
                }

                // Otherwise perform a global search.
                if (component != null)
                {
                    var globalSearchResult = component.Entity.EntityManager.GetAll(property.PropertyType).FirstOrDefault();
                    if (globalSearchResult != null)
                    {
                        property.SetValue(target, globalSearchResult);
                        continue;
                    }
                }             
                Console.Error.WriteLine($"Dependency could not be resolved: {property.PropertyType}");
            }
        }

        public static void AddService<T>(this ServiceContainer serviceContainer, T instance)
        {
            serviceContainer.AddService(typeof(T), instance);
        }
    }
}
