using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using Forge.Core.Rendering;
using Forge.Core.Scenes;
using Forge.Core.Services;
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
        public EntityManager EntityManager { get; }

        public GameLoop GameLoop { get; }

        /// <summary>
        /// Manages the active scenes.
        /// </summary>
        public SceneManager SceneManager { get; }

        /// <summary>
        /// Manages the active services.
        /// </summary>
        public ServiceManager ServiceManager { get; }

        public ForgeEngine(IList<Type> indexTypes)
        {
            EntityManager = new EntityManager(new EntityPool(ushort.MaxValue), indexTypes, ServiceContainer);
            GameLoop = new GameLoop(EntityManager);
            SceneManager = new SceneManager(EntityManager);
            ServiceManager = new ServiceManager(EntityManager, ServiceContainer);

            // Register services.
            ServiceContainer.AddService<EntityManager>(EntityManager);
            ServiceContainer.AddService<ServiceManager>(ServiceManager);
            ServiceContainer.AddService<SceneManager>(SceneManager);
        }

        public void Initialise(GraphicsDeviceManager graphics, ContentManager content, GameWindow gameWindow)
        {
            _graphics = graphics;
            if (_graphics != null)
            {
                _spriteBatch = new SpriteBatch(_graphics.GraphicsDevice);
                ServiceContainer.AddService<GraphicsDeviceManager>(graphics);
                ServiceContainer.AddService<GraphicsDevice>(graphics.GraphicsDevice);
            }
            if (content != null)
            {
                ServiceContainer.AddService<ContentManager>(content);
            }
            if (gameWindow != null)
            {
                ServiceContainer.AddService<GameWindow>(gameWindow);
            }

            Initialised?.Invoke();
        }

        public void Draw(GameTime gameTime)
        {
            if (_graphics == null)
            {
                return;
            }
            var context = new RenderContext(gameTime, _spriteBatch, _graphics?.GraphicsDevice);

            var renderModifiers = EntityManager.GetAll<IRenderModifier>()
                .ToList();
            renderModifiers.Sort((x, y) => (int)(x.ApplicationOrder - y.ApplicationOrder));
            foreach (var renderModifier in renderModifiers)
            {
                renderModifier.Apply(context);
            }

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
            // Runs the step on each thread for the phase. Will not return until all have completed.
            GameLoop.Execute();
        }
    }
}
