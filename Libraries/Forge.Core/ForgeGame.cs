using Forge.Core.Engine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Forge.Core
{
    public class ForgeGame : Game
    {
        private readonly ForgeEngine _engine;
        private readonly GraphicsDeviceManager _graphics;

        public ForgeGame(ForgeEngine engine)
        {
            _engine = engine;
            _graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            _engine.Tick();
        }
    }
}
