using Forge.Core.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Forge.UI.Glass
{
    public class RenderContext
    {
        public SpriteBatch SpriteBatch { get; }
        public GameTime GameTime { get; }
        public ResourceManager<SpriteFont> Fonts { get; }

        public RenderContext(SpriteBatch spriteBatch, GameTime gameTime, ResourceManager<SpriteFont> fonts)
        {
            SpriteBatch = spriteBatch;
            GameTime = gameTime;
            Fonts = fonts;
        }
    }
}
