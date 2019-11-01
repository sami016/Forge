using Forge.Core.Components;
using Forge.Core.Resources;
using Forge.Core.Scenes;
using Forge.UI.Glass;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SampleGame.Templates;
using System;
using System.Collections.Generic;
using System.Text;

namespace SampleGame
{
    public class MenuScene : Scene
    {
        [Inject] UserInterfaceManager UserInterfaceManager { get; set; }
        [Inject] ResourceManager<SpriteFont> Fonts { get; set; }
        [Inject] ResourceManager<Texture2D> Textures { get; set; }
        [Inject] ContentManager Content { get; set; }

        public override void Initialise()
        {
            UserInterfaceManager.Create(new SplashTemplate("Welcome to Forge"));

            Fonts.Load("Default", Content.Load<SpriteFont>("Font/Default"));

            Textures.Load("Rocket", Content.Load<Texture2D>("Icon/Rocket"));
            Textures.Load("Settings", Content.Load<Texture2D>("Icon/Settings"));
            Textures.Load("Globe", Content.Load<Texture2D>("Icon/Globe"));

        }
    }
}
