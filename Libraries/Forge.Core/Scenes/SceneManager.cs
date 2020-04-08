using Forge.Core.Engine;
using Forge.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Forge.Core.Scenes
{
    /// <summary>
    /// Scene manager is for managing content grouped together into scenes.
    /// </summary>
    public class SceneManager
    {

        private readonly EntityManager _entityManager;

        public Scene Loaded { get; private set; }

        public SceneManager(EntityManager EntityManager)
        {
            _entityManager = EntityManager;
        }

        public void ClearScene()
        {
            if (Loaded != null)
            {
                var loadedEnt = Loaded.Entity;
                Loaded.Dispose();
                loadedEnt.Delete();
                Loaded = null;
            }
        }

        public void SetScene<T>(T scene)
            where T : Scene
        {
            ClearScene();
            var sceneEnt = _entityManager.Create();
            sceneEnt.Add(scene);
            sceneEnt.Spawn();
            Loaded = scene;
        }
    }
}
