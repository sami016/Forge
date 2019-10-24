using Forge.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Forge.Core.Scenes
{
    public class SceneManager
    {
        private readonly IServiceProvider _serviceProvider;
        public Scene Loaded { get; private set; }

        public SceneManager(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void SetScene(Scene scene)
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
        }
    }
}
