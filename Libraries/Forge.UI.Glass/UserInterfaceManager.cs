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
using System.Text;

namespace Forge.UI.Glass
{
    public delegate void UIDispose();
    public class UserInterfaceManager : Component, IRenderable, ITick
    {
        private readonly IList<ITemplate> _templates = new List<ITemplate>();

        public uint RenderOrder { get; set; } = 150;
        public bool AutoRender { get; } = true;

        public IEnumerable<ITemplate> TemplateLayers => _templates;
        public int ActiveTemplateCount => _templates.Count;

        [Inject] public GraphicsDevice GraphicsDevice { get; set; }
        [Inject] public ResourceManager<SpriteFont> Fonts { get; set; }
        [Inject] public ResourceManager<Color> Colours { get; set; }
        [Inject] public ResourceManager<Texture2D> Textures { get; set; }
        [Inject] public RenderResources RenderPrimitives { get; set; }

        public UIDispose Create(ITemplate template)
        {
            template.Position = GraphicsDevice.Viewport.Bounds;
            _templates.Add(template);
            template.Initialise();

            return () =>
            {
                _templates.Remove(template);
            };
        }

        public void Render(RenderContext context)
        {
            //TODO use engine renderable interface.
            foreach (var element in _templates)
            {
                element.Position = GraphicsDevice.Viewport.Bounds;
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
                        context.GraphicsDevice.Viewport.Bounds
                    )
                );
            }
        }

        public void Tick(TickContext context)
        {
            foreach (var element in _templates)
            {
                element.Tick(
                   context
                );
            }
        }
    }
}
