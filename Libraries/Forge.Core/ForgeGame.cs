using Forge.Core.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Forge.Core
{
    public class ForgeGame : Game
    {
        private readonly ForgeEngine _engine;
        private readonly GraphicsDeviceManager _graphics;

        public Color BackgroundColour { get; set; } = Color.Black;

        public ForgeGame(ForgeEngine engine)
        {
            _engine = engine;
            _graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();
            _engine.Initialise(_graphics, Content, Window);

            _graphics.PreparingDeviceSettings += (object s, PreparingDeviceSettingsEventArgs args) =>
            {
                args.GraphicsDeviceInformation.PresentationParameters.RenderTargetUsage = RenderTargetUsage.PreserveContents;
            };
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            _engine.Tick(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(BackgroundColour);
            base.Draw(gameTime);
            _engine.Draw(gameTime);
        }
    }
}
