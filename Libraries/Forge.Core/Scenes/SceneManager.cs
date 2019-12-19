using Forge.Core.Engine;
using Forge.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Forge.Core.Scenes
{
    public class SceneManager
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly EntityManager _entityManager;

        public Scene Loaded { get; private set; }

        public SceneManager(IServiceProvider serviceProvider, EntityManager EntityManager)
        {
            _serviceProvider = serviceProvider;
            _entityManager = EntityManager;
        }

        public void SetScene(Scene scene)
        {
            if (Loaded != null)
            {
                Loaded.Dispose();
            }
            Loaded = scene;
            if (scene != null)
            {
                _serviceProvider.Inject(scene);
            }
        }
    }
}
