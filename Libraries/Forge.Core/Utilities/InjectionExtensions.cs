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
            if (target is IInit init)
            {
                init.Initialise();
            }
        }

        public static void AddService<T>(this ServiceContainer serviceContainer, T instance)
        {
            serviceContainer.AddService(typeof(T), instance);
        }
    }
}
