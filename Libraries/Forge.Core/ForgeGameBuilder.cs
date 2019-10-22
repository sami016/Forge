using Forge.Core.Components;
using Forge.Core.Engine;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Forge.Core
{
    public class ForgeGameBuilder
    {
        private IServiceCollection _serviceProvider;
        private IList<Type> _indexInterfaces = new List<Type>();
        private IList<(Type type, Func<IComponent> factory)> _singletonCreators = new List<(Type, Func<IComponent>)>();

        public ForgeGameBuilder()
        {

        }

        public void AddSingleton<T>(Func<T> singletonCreator)
            where T : IComponent
        {
            _singletonCreators.Add((typeof(T), () => singletonCreator()));
        }

        public void IndexInterface<T>()
        {
            var type = typeof(T);
            if (!type.IsInterface)
            {
                throw new Exception($"Type {type.Name} is not an interface");
            }
            if (!_indexInterfaces.Contains(type))
            {
                _indexInterfaces.Add(type);
            }
        }

        public ForgeGameBuilder UseServiceContainer()
        {
            return this;
        }

        public ForgeGame Create()
        {
            var engine = new ForgeEngine(Environment.ProcessorCount);
            return new ForgeGame(engine);
        }
    }
}
