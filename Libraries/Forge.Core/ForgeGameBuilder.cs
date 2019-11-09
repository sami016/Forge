using Forge.Core.Components;
using Forge.Core.Engine;
using Forge.Core.Scenes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Forge.Core
{
    public class ForgeGameBuilder
    {
        private IServiceCollection _serviceProvider;
        private IList<Type> _indexInterfaces = new List<Type>();
        private IList<(Type type, Func<object> factory)> _singletonCreators = new List<(Type, Func<object>)>();
        private Func<Scene> _initialSceneFactory = () => null;
        private Color? _backgroundColour = null;

        public ForgeGameBuilder()
        {

        }

        public ForgeGameBuilder AddSingleton<T>(Func<T> singletonCreator)
        {
            _singletonCreators.Add((typeof(T), () => singletonCreator()));
            return this;
        }

        public ForgeGameBuilder IndexInterface<T>()
        {
            var type = typeof(T);
            //if (!type.IsInterface)
            //{
            //    throw new Exception($"Type {type.Name} is not an interface");
            //}
            if (!_indexInterfaces.Contains(type))
            {
                _indexInterfaces.Add(type);
            }
            return this;
        }

        public ForgeGameBuilder WithInitialScene(Func<Scene> sceneFactory)
        {
            _initialSceneFactory = sceneFactory;

            return this;
        }

        public ForgeGameBuilder UseBackgroundRefreshColour(Color color)
        {
            _backgroundColour = color;

            return this;
        }

        public ForgeGameBuilder UseServiceContainer()
        {
            return this;
        }

        public ForgeGame Create()
        {
            var engine = new ForgeEngine(Environment.ProcessorCount, _indexInterfaces);
            engine.Initialised += () =>
            {
                foreach (var entry in _singletonCreators)
                {
                    var entity = engine.EntityManager.Create();
                    var component = entry.factory();
                    entity.Add(component as IComponent);
                    engine.ServiceContainer.AddService(entry.type, component);
                }
                engine.SceneManager.SetScene(_initialSceneFactory());
            };
            var game = new ForgeGame(engine);
            if (_backgroundColour.HasValue)
            {
                game.BackgroundColour = _backgroundColour.Value;
            }
            return game;
        }
    }
}
