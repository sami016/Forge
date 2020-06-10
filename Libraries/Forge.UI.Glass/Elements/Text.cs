using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Forge.UI.Glass.Elements
{
    public class Text : Primitive
    {
        public enum Positioning : byte
        {
            Left,
            Center,
            Right
        }

        public string Value { get; set; }
        public string Font { get; set; }
        public Color? Colour { get; set; }
        public Positioning TextAlign { get; set; }

        public Text(params IElement[] children) : base(children)
        {
        }

        public Text(string value, params IElement[] children) : base(children)
        {
            Value = value;
        }

        public Text(string value, string font, params IElement[] children) : base(children)
        {
            Value = value;
            Font = font;
        }

        public override void Render(UIRenderContext context)
        {
            var screenPosition = context.RenderPort;


            var font = context.Fonts.Get(Font ?? "Default");
            if (font == null)
            {
                font = context.Fonts.Get("Default");
            }
            var width = font.MeasureString(Value).X;

            context.SpriteBatch.Begin(transformMatrix: context.Transform);

            switch (TextAlign)
            {
                case Positioning.Center:
                    context.SpriteBatch.DrawString(font, Value, screenPosition.Location.ToVector2() - new Vector2(width / 2f, 0), Colour ?? Color.White);
                    break;
                case Positioning.Left:
                    context.SpriteBatch.DrawString(font, Value, screenPosition.Location.ToVector2(), Colour ?? Color.White);
                    break;
                case Positioning.Right:
                    context.SpriteBatch.DrawString(font, Value, screenPosition.Location.ToVector2() - new Vector2(width, 0), Colour ?? Color.White);
                    break;
            }
            context.SpriteBatch.End();
        }
    }
}
