using Forge.Core.Rendering;
using Forge.Core.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Forge.UI.Glass
{
    public class UIRenderContext
    {
        public GraphicsDevice GraphicsDevice { get; }
        public SpriteBatch SpriteBatch { get; }
        public GameTime GameTime { get; }
        public ResourceManager<SpriteFont> Fonts { get; }
        public ResourceManager<Color> Colours { get; }
        public ResourceManager<Texture2D> Textures { get; }
        public RenderResources RenderPrimitives { get; }
        public Rectangle RenderPort { get; }


        public UIRenderContext(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch, GameTime gameTime, ResourceManager<SpriteFont> fonts, ResourceManager<Color> colours, ResourceManager<Texture2D> textures, RenderResources renderPrimitives, Rectangle renderPort)
        {
            GraphicsDevice = graphicsDevice;
            SpriteBatch = spriteBatch;
            GameTime = gameTime;
            Fonts = fonts;
            Colours = colours;
            Textures = textures;
            RenderPrimitives = renderPrimitives;
            RenderPort = renderPort;
        }

        public Vector2 GetGlobalPosition(Vector2 pos)
        {
            return pos + RenderPort.Location.ToVector2();
        }

        public UIRenderContext SubContext(Rectangle subPosition)
        {
            var newRenderPort = new Rectangle(RenderPort.X + subPosition.X, RenderPort.Y + subPosition.Y, subPosition.Width, subPosition.Height);
            return new UIRenderContext(GraphicsDevice, SpriteBatch, GameTime, Fonts, Colours, Textures, RenderPrimitives, newRenderPort);
        }
    }
}
