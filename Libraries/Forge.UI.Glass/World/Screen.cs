using Forge.Core;
using Forge.Core.Components;
using Forge.Core.Interfaces;
using Forge.Core.Rendering;
using Forge.Core.Resources;
using Forge.UI.Glass.Templates;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Forge.UI.Glass.World
{
    /// <summary>
    /// A screen for rendering UI onto.
    /// </summary>
    public class Screen : Component, IInit, ITick, IPrerenderable
    {
        private RenderTarget2D _renderTarget;
        private readonly Texture2D _texture;
        private readonly ITemplate _template;
        private readonly int _screenWidth;
        private readonly int _screenHeight;

        public Texture2D Texture => _renderTarget;
        public Matrix? Transform { get; set; } = null;
        public uint RenderOrder { get; set; } = 150;
        public bool TickEnabled { get; } = true;
        public Color TextureColor { get; set; } = Color.White;

        public bool AutoRender { get; set; } = true;
        private bool _manualRerender;

        [Inject] public GraphicsDevice GraphicsDevice { get; set; }
        [Inject] public ResourceManager<SpriteFont> Fonts { get; set; }
        [Inject] public ResourceManager<Color> Colours { get; set; }
        [Inject] public ContentManager ContentManager { get; set; }
        [Inject] public RenderResources RenderPrimitives { get; set; }
        [Inject] IServiceProvider ServiceProvider { get; set; }


        public Screen(ITemplate template, int screenWidth, int screenHeight)
        {
            _template = template;
            _screenWidth = screenWidth;
            _screenHeight = screenHeight;
            _manualRerender = true;
        }

        public void Initialise()
        {
            var uiInitialiseContext = new UIInitialiseContext(GraphicsDevice, ServiceProvider);

            _template.Initialise(uiInitialiseContext);
            _renderTarget = new RenderTarget2D(GraphicsDevice, _screenWidth, _screenHeight, false, SurfaceFormat.Color, DepthFormat.None);
        }

        public Screen(ITemplate template, Texture2D texture): this(template, texture.Width, texture.Height)
        {
            _texture = texture;
        }

        public void Rerender()
        {
            _manualRerender = true;
        }

        public void Prerender(RenderContext context)
        {
            if (AutoRender
                || _manualRerender)
            {
                GraphicsDevice.SetRenderTarget(_renderTarget);

                if (_texture != null)
                {
                    context.SpriteBatch.Begin();
                    context.SpriteBatch.Draw(_texture, new Rectangle(0, 0, _screenWidth, _screenHeight), TextureColor);
                    context.SpriteBatch.End();
                }
                _template.Render(
                    new UIRenderContext
                    (
                        GraphicsDevice,
                        context.SpriteBatch,
                        context.GameTime,
                        Fonts,
                        Colours,
                        ContentManager,
                        RenderPrimitives,
                        _template.Position,
                        Transform
                    )
                );
                GraphicsDevice.SetRenderTarget(null);
                if (_manualRerender)
                {
                    _manualRerender = false;
                }
            }
        }

        public void Tick(TickContext context)
        {
            _template.Tick(
               context
            );
        }
    }
}
