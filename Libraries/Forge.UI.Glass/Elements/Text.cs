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
        public bool VerticalCenter { get; set; }

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
            var size = font.MeasureString(Value);

            context.SpriteBatch.Begin(transformMatrix: context.Transform);

            var pos = screenPosition.Location.ToVector2();

            switch (TextAlign)
            {
                case Positioning.Center:
                    pos -= new Vector2(size.X / 2f, 0);
                    break;
                case Positioning.Left:
                    break;
                case Positioning.Right:
                    pos -= new Vector2(size.X, 0);
                    break;
            }

            if (VerticalCenter)
            {
                pos -= new Vector2(0, size.Y / 2);
            }

            context.SpriteBatch.DrawString(font, Value, pos, Colour ?? Color.White);
            context.SpriteBatch.End();
        }
    }
}
