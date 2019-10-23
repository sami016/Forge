using System;
using System.Collections.Generic;
using System.Text;

namespace Forge.Core.Scenes
{
    public class SceneManager
    {
        public Scene Loaded { get; private set; }

        public void SetScene(Scene scene)
        {
            if (Loaded != null)
            {
                Loaded.Dispose();
            }
            Loaded = scene;
        }
    }
}
