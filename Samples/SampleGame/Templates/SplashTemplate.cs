using Forge.UI.Glass.Elements;
using Forge.UI.Glass.Stylings;
using Forge.UI.Glass.Templates;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace SampleGame.Templates
{
    public class SplashTemplate : Template
    {
        private readonly string _message;

        public SplashTemplate(string message)
        {
            _message = message;
        }

        public override IElement Evaluate()
        {
            return new Pane(
                new Text()
                {
                    Value = _message
                }    
            )
            {
                Background = new ColourBackgroundStyling
                {
                    Colour = Color.SlateGray
                },
                Position = new Rectangle(100, 100, 300, 150)
            };
        }
    }
}
