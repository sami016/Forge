using Forge.Core;
using Forge.Core.Components;
using Forge.Core.Interfaces;
using Forge.Core.Rendering;
using Forge.Core.Resources;
using Forge.UI.Glass.Templates;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Forge.UI.Glass
{
    public delegate void UIDispose();
    public class UserInterfaceManager : Component, IRenderable, ITick
    {
        private readonly IDictionary<ITemplate, int> _templates = new Dictionary<ITemplate, int>();

        public uint RenderOrder { get; set; } = 150;
        public bool AutoRender { get; } = true;

        public IEnumerable<ITemplate> TemplateLayers => _templates.Keys;
        public int ActiveTemplateCount => _templates.Count;

        [Inject] public GraphicsDevice GraphicsDevice { get; set; }
        [Inject] public ResourceManager<SpriteFont> Fonts { get; set; }
        [Inject] public ResourceManager<Color> Colours { get; set; }
        [Inject] public ResourceManager<Texture2D> Textures { get; set; }
        [Inject] public RenderResources RenderPrimitives { get; set; }
        [Inject] IServiceProvider ServiceProvider { get; set; }

        public UIDispose Create(ITemplate template, int layer = 100)
        {
            var uiInitialiseContext = new UIInitialiseContext(GraphicsDevice, ServiceProvider);

            template.Position = GraphicsDevice.Viewport.Bounds;
            _templates[template] = layer;
            template.Initialise(uiInitialiseContext);

            return () =>
            {
                _templates.Remove(template);
            };
        }

        public void Render(RenderContext context)
        {
            //TODO use engine renderable interface.
            foreach (var element in _templates
                .OrderBy(x => x.Value)
                .Select(x => x.Key)
                .ToArray()
            )
            {
                //element.Position = GraphicsDevice.Viewport.Bounds;
                element.Render(
                    new UIRenderContext
                    (
                        GraphicsDevice,
                        context.SpriteBatch,
                        context.GameTime,
                        Fonts,
                        Colours,
                        Textures,
                        RenderPrimitives,
                        element.Position
                    )
                );
            }
        }

        public void Tick(TickContext context)
        {
            foreach (var element in _templates.Keys.ToArray())
            {
                element.Tick(
                   context
                );
            }
        }
    }
}
