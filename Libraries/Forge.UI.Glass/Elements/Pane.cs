using Forge.UI.Glass.Stylings;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Forge.UI.Glass.Elements
{
    /// <summary>
    /// A pane is a primitive container element.
    /// </summary>
    public class Pane : Primitive
    {
        public IBackgroundStyling Background { get; set; }
        public Rectangle Position { get; set; }

        public Pane(params IElement[] children) : base(children)
        {
        }

        public override void Render(RenderContext context)
        {
            //
            var spriteBatch = context.SpriteBatch;

            var screenPosition = Position;
            switch (Background)
            {
                case null:
                    break;
                case ColourBackgroundStyling colourBackground:
                    var colour = !string.IsNullOrEmpty(colourBackground.ColourResource) 
                        ? context.Colours.Get(colourBackground.ColourResource) 
                        : colourBackground.Colour;
                    colour = colour ?? colourBackground.Colour ?? Color.Purple;
                    spriteBatch.Begin();
                    spriteBatch.Draw(
                        null,
                        screenPosition,
                        null,
                        Color.White,
                        0,
                        Vector2.Zero,
                        SpriteEffects.None,
                        0f
                    );
                    spriteBatch.End();
                    break;
                case ImageBackgroundStyling imageBackground:
                    break;
            }

            // Render children.
            foreach (var child in Children)
            {
                child.Render(context);
            }
        }
    }
}
