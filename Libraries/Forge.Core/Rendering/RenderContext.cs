using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Forge.Core.Rendering
{
    public class RenderContext
    {
        
        public RenderContext(GameTime gameTime, SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            GameTime = gameTime;
            SpriteBatch = spriteBatch;
            GraphicsDevice = graphicsDevice;
        }

        public GameTime GameTime { get; }
        public SpriteBatch SpriteBatch { get; }
        public GraphicsDevice GraphicsDevice { get; }
    }
}
