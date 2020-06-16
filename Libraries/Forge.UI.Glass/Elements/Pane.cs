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

        public Pane(params IElement[] children) : base(children)
        {
        }

        public override void Render(UIRenderContext context)
        {
            //
            var spriteBatch = context.SpriteBatch;

            var screenPosition = context.RenderPort;
            switch (Background)
            {
                case null:
                    break;
                case ColourBackgroundStyling colourBackground:
                    var colour = !string.IsNullOrEmpty(colourBackground.ColourResource) 
                        ? context.Colours.Get(colourBackground.ColourResource) 
                        : colourBackground.Colour;
                    colour = colour ?? colourBackground.Colour ?? Color.Purple;
                    spriteBatch.Begin(transformMatrix: context.Transform);
                    spriteBatch.Draw(
                        context.RenderPrimitives.WhiteTexture,
                        screenPosition,
                        null,
                        colour.Value,
                        0,
                        Vector2.Zero,
                        SpriteEffects.None,
                        0f
                    );
                    spriteBatch.End();
                    break;
                case ImageBackgroundStyling imageBackground:
                    var texture = !string.IsNullOrEmpty(imageBackground.ImageResource)
                        ? context.ContentManager.Load<Texture2D>(imageBackground.ImageResource)
                        : imageBackground.Image;
                    if (texture != null)
                    {
                        spriteBatch.Begin(transformMatrix: context.Transform);
                        spriteBatch.Draw(
                            texture,
                            screenPosition,
                            null,
                            imageBackground.Colour,
                            0,
                            Vector2.Zero,
                            SpriteEffects.None,
                            0f
                        );
                        spriteBatch.End();
                    }
                    break;
            }

            // Render children.
            foreach (var child in Children)
            {
                child.Render(context.SubContext(child.Position));
            }
        }
    }
}
