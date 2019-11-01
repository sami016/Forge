using Forge.Core.Components;
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
    public class UserInterfaceManager : Component, IRenderable
    {
        private readonly IList<ITemplate> _templates = new List<ITemplate>();

        public IEnumerable<ITemplate> TemplateLayers => _templates;
        public int ActiveTemplateCount => _templates.Count;

        [Inject] public ResourceManager<SpriteFont> Fonts { get; set; }
        [Inject] public ResourceManager<Color> Colours { get; set; }
        [Inject]  public ResourceManager<Texture2D> Textures { get; set; }
        [Inject] public RenderResources RenderPrimitives { get; set; }

        public UIDispose Create(ITemplate template)
        {
            template.Initialise();
            template.Reevaluate();
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
                element.Render(
                    new UIRenderContext
                    (
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
    }
}
