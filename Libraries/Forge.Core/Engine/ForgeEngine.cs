using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Text;
using Forge.Core.Rendering;
using Forge.Core.Scenes;
using Forge.Core.Utilities;
using Microsoft.Xna.Framework;

namespace Forge.Core.Engine
{
    public class ForgeEngine
    {
        public ServiceContainer ServiceContainer { get; } = new ServiceContainer();
        public SideEffectManager SideEffectManager { get; set; }
        public EntityManager EntityManager { get; }
        public GameLoop GameLoop { get; }
        public ExecutePool ExecutePool { get; }
        public SceneManager SceneManager { get; }

        public IList<GameLoopPhase> Phases { get; set; }

        public ForgeEngine(int threadCount)
        {
            SideEffectManager = new SideEffectManager(threadCount);
            EntityManager = new EntityManager(new ParallelCollection<Entity>(threadCount, ushort.MaxValue), SideEffectManager);
            GameLoop = new GameLoop(EntityManager, SideEffectManager);
            ExecutePool = new ExecutePool(threadCount, GameLoop);
            SceneManager = new SceneManager(ServiceContainer);

            // Register services.
            ServiceContainer.AddService<SideEffectManager>(SideEffectManager);
            ServiceContainer.AddService<EntityManager>(EntityManager);
            ServiceContainer.AddService<SceneManager>(SceneManager);

            Phases = new List<GameLoopPhase>()
            {
                GameLoopPhase.Tick,
                GameLoopPhase.Update
            };
        }

        public void Draw(GameTime gameTime)
        {
            var renderables = EntityManager.GetAll<IRenderable>();
            foreach (var renderable in renderables)
            {
                renderable.Render(gameTime);
            }
        }

        /// <summary>
        /// Runs a single update tick.
        /// </summary>
        public void Tick()
        {
            // Execute configured phases on each tick.
            foreach (var phase in Phases)
            {
                // Set the current phase on the game loop.
                GameLoop.Phase = phase;
                // Runs the step on each thread for the phase. Will not return until all have completed.
                ExecutePool.RunStep();
            }
        }
    }
}
