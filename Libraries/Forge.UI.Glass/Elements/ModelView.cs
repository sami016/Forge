using Forge.Core.Rendering;
using Forge.UI.Glass.Stylings;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Forge.UI.Glass.Elements
{
    /// <summary>
    /// A pane is a primitive container element.
    /// </summary>
    public class ModelView : Primitive
    {

        private RenderTarget2D _renderTarget;
        public Color Colour { get; set; } = Color.Transparent;
        public IBackgroundStyling Background { get; set; }
        public Matrix? View { get; set; }
        public Matrix? Projection { get; set; }
        public IRenderable Renderable { get; set; }
        public Action<RenderContext> RenderFunc { get; set; }
        public bool SingleRender { get; set; } = true;

        private bool _rendered = false;

        public ModelView() : base(new IElement[0])
        {
        }

        public override void Render(UIRenderContext context)
        {
            var screenPosition = context.RenderPort;
            if (View == null)
            {
                View = Matrix.CreateLookAt(new Vector3(1f, 1f, 1f), Vector3.Zero, Vector3.Up);
            }
            if (Projection == null)
            {
                Projection = Matrix.CreateOrthographicOffCenter(-1, 1, -1, 1, 0.001f, 10000f);
                    //Matrix.CreatePerspective(context.GraphicsDevice.Viewport.Width, context.GraphicsDevice.Viewport.Height, 0.001f, 10000f);
            }
            if (_renderTarget == null
                || _renderTarget.Height != screenPosition.Height
                || _renderTarget.Width != screenPosition.Width)
            {
                _rendered = false;
                _renderTarget = new RenderTarget2D(context.GraphicsDevice, screenPosition.Width, screenPosition.Height, false, SurfaceFormat.Vector4, DepthFormat.Depth24, 0, RenderTargetUsage.PreserveContents);
            }
            if (!SingleRender || !_rendered)
            {
                context.GraphicsDevice.SetRenderTarget(_renderTarget);
                context.GraphicsDevice.Clear(Colour);

                var renderContext = new RenderContext(context.GameTime, context.SpriteBatch, context.GraphicsDevice)
                {
                    Projection = Projection,
                    View = View,
                };
                Renderable?.Render(renderContext);
                RenderFunc?.Invoke(renderContext);
                context.GraphicsDevice.SetRenderTarget(null);
                _rendered = true;
            }
            context.SpriteBatch.Begin();
            context.SpriteBatch.Draw(_renderTarget, screenPosition, Color.White);
            context.SpriteBatch.End();
        }
    }
}
