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

        public IBackgroundStyling Background { get; set; }
        public Matrix? View { get; set; }
        public Matrix? Projection { get; set; }
        public IRenderable Renderable { get; set; }

        public ModelView() : base(new IElement[0])
        {
        }

        public override void Render(UIRenderContext context)
        {
            if (View == null)
            {
                View = Matrix.CreateLookAt(Vector3.One * 3, Vector3.Zero, Vector3.Up);
            }
            if (Projection == null)
            {
                Projection = Matrix.CreatePerspective(Position.Width, Position.Height, 0.001f, 10000f);
            }
            if (_renderTarget == null)
            {
                _renderTarget = new RenderTarget2D(context.GraphicsDevice, Position.Width, Position.Height);
            }
            context.GraphicsDevice.SetRenderTarget(_renderTarget);
            Renderable?.Render(new RenderContext(context.GameTime, context.SpriteBatch, context.GraphicsDevice)
            {
                Projection = Projection,
                View = View,
            });
            context.GraphicsDevice.SetRenderTarget(null);
        }
    }
}
