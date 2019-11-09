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
            _entityManager.Update(() =>
            {
                if (Loaded != null)
                {
                    Loaded.Dispose();
                }
                if (scene != null)
                {
                    _serviceProvider.Inject(scene);
                }
                Loaded = scene;
            });
        }
    }
}
