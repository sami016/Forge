using Forge.Core.Components;
using Forge.Core.Scenes;
using Forge.UI.Glass;
using SampleGame.Templates;
using System;
using System.Collections.Generic;
using System.Text;

namespace SampleGame
{
    public class MenuScene : Scene
    {
        [Inject] UserInterfaceManager UserInterfaceManager { get; set; }

        public override void Initialise()
        {
            UserInterfaceManager.Create(new SplashTemplate("Welcome to Forge"));
        }
    }
}
