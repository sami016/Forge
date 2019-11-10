using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using Forge.Core.Rendering;
using Forge.Core.Scenes;
using Forge.Core.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Forge.Core.Engine
{
    public class ForgeEngine
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public event Action Initialised;

        public ServiceContainer ServiceContainer { get; } = new ServiceContainer();
        public SideEffectManager SideEffectManager { get; set; }
        public EntityManager EntityManager { get; }

        public GameLoop GameLoop { get; }
        public ExecutePool ExecutePool { get; }
        public SceneManager SceneManager { get; }

        public IList<GameLoopPhase> Phases { get; set; }

        public ForgeEngine(int threadCount, IList<Type> indexTypes)
        {
            SideEffectManager = new SideEffectManager(threadCount);
            EntityManager = new EntityManager(new EntityPool(threadCount, ushort.MaxValue, indexTypes), indexTypes, SideEffectManager, ServiceContainer);
            GameLoop = new GameLoop(EntityManager, SideEffectManager);
            ExecutePool = new ExecutePool(threadCount, GameLoop);
            SceneManager = new SceneManager(ServiceContainer, EntityManager);

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

        public void Initialise(GraphicsDeviceManager graphics, ContentManager content, GameWindow gameWindow)
        {
            _graphics = graphics;
            _spriteBatch = new SpriteBatch(_graphics.GraphicsDevice);
            ServiceContainer.AddService<GraphicsDeviceManager>(graphics);
            ServiceContainer.AddService<GraphicsDevice>(graphics.GraphicsDevice);
            ServiceContainer.AddService<ContentManager>(content);
            ServiceContainer.AddService<GameWindow>(gameWindow);

            Initialised?.Invoke();
        }

        public void Draw(GameTime gameTime)
        {
            if (_graphics == null)
            {
                return;
            }
            var context = new RenderContext(gameTime, _spriteBatch, _graphics?.GraphicsDevice);

            var renderables = EntityManager.GetAll<IRenderable>()
                .Where(x => x.AutoRender)
                .ToList();
            renderables.Sort((x, y) => (int)(x.RenderOrder - y.RenderOrder));
            foreach (var renderable in renderables)
            {
                renderable.Render(context);
            }
        }

        /// <summary>
        /// Runs a single update tick.
        /// </summary>
        public void Tick(GameTime gameTime)
        {
            GameLoop.GameTime = gameTime;
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
