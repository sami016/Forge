using Forge.UI.Glass.Elements;
using Forge.UI.Glass.Interaction;
using Forge.UI.Glass.Stylings;
using Forge.UI.Glass.Templates;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
                },
                new Pane(
                )
                {
                    Background = new ImageBackgroundStyling
                    {
                        ImageResource = "Rocket"
                    },
                    Position = new Rectangle(100, 100, 50, 50),
                    Init = el => el.Events.Subscribe<ClickUIEvent>(RocketClicked)                  
                },
                new Pane(
                )
                {
                    Background = new ImageBackgroundStyling
                    {
                        ImageResource = "Globe"
                    },
                    Position = new Rectangle(200, 120, 50, 50)
                },
                new Pane(
                )
                {
                    Background = new ImageBackgroundStyling
                    {
                        ImageResource = "Settings"
                    },
                    Position = new Rectangle(300, 140, 50, 50)
                }
            )
            {
                Background = new ColourBackgroundStyling
                {
                    Colour = Color.SlateGray
                },
                //Background = new ImageBackgroundStyling
                //{
                //    ImageResource = "Rocket"
                //},
                Position = new Rectangle(100, 100, 150, 150)
            };
        }

        private void RocketClicked(ClickUIEvent @event)
        {
            Debug.WriteLine("Rocket clicked");
        }
    }
}
