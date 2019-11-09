using Forge.Core.Components;
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
            InjectServiceProvider(serviceProvider, target);
            if (target is IComponent component)
            {
                InjectInternally(component);
            }
            if (target is IInit init)
            {
                init.Initialise();
            }
        }

        private static void InjectInternally(this IComponent target)
        {
            var type = target.GetType();
            foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(x => x.GetCustomAttribute<InjectAttribute>() != null))
            {
                if (!property.CanWrite)
                {
                    continue;
                }
                if (target.Entity.Has(property.PropertyType))
                {
                    property.SetValue(target, target.Entity.Get(property.PropertyType));
                }
            }
        }

        private static void InjectServiceProvider(this IServiceProvider serviceProvider, object target)
        {
            var type = target.GetType();
            foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(x => x.GetCustomAttribute<InjectAttribute>() != null))
            {
                if (!property.CanWrite)
                {
                    continue;
                }
                property.SetValue(target, serviceProvider.GetService(property.PropertyType));
            }
        }

        public static void AddService<T>(this ServiceContainer serviceContainer, T instance)
        {
            serviceContainer.AddService(typeof(T), instance);
        }
    }
}
