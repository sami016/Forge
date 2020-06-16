using Forge.Core.Components;
using Forge.Core.Interfaces;
using Forge.Core.Rendering;
using Forge.Core.Resources;
using Forge.UI.Glass.Elements;
using Forge.UI.Glass.Templates;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Forge.UI.Glass.Rendering
{
    public class OverlayRenderer : Component, IInit, IRenderable
    {
        private readonly ITemplate _template;
        private OverlayTemplate _overlayTemplate;
        private UIDispose _uiDisposal;

        [Inject] Transform Transform { get; set; }
        [Inject] GraphicsDevice GraphicsDevice { get; set; }
        [Inject] public ResourceManager<SpriteFont> Fonts { get; set; }
        [Inject] public ResourceManager<Color> Colours { get; set; }
        [Inject] public ContentManager ContentManager { get; set; }
        [Inject] public RenderResources RenderPrimitives { get; set; }
        [Inject] IServiceProvider ServiceProvider { get; set; }
        [Inject] UserInterfaceManager UserInterfaceManager { get; set; }

        public uint RenderOrder { get; } = 200;

        public bool AutoRender { get; } = true;

        public OverlayRenderer(ITemplate template)
        {
            _template = template;
        }

        public void Render(RenderContext context)
        {
            var iniPosition = _template.Current.Position;
            var pos = Transform.Location;
            var screenPos3D = context.GraphicsDevice.Viewport.Project(pos, context.Projection.Value, context.View.Value, Matrix.Identity);
            var screenPos = new Vector2(screenPos3D.X, screenPos3D.Y);
            var rectangle = new Rectangle(
                (int)screenPos.X - iniPosition.Width / 2, 
                (int)screenPos.Y - iniPosition.Height / 2, 
                _template.Position.Width, 
                _template.Position.Height
            );
            _template.Position = rectangle;
        }

        public void Initialise()
        {
            _overlayTemplate = new OverlayTemplate(_template);
            _uiDisposal = UserInterfaceManager.Create(_overlayTemplate);
        }

        public override void Dispose()
        {
            base.Dispose();

            _uiDisposal?.Invoke();
        }
    }

    public class OverlayTemplate : Template
    {
        private readonly ITemplate _template;

        public OverlayTemplate(ITemplate template)
        {
            _template = template;
        }

        public override IElement Evaluate()
        {
            return new Pane(_template);
        }
    }
}
