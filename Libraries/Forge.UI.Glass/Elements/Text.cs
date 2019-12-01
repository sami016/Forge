using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Forge.UI.Glass.Elements
{
    public class Text : Primitive
    {
        public string Value { get; set; }
        public string Font { get; set; }
        public Color? Colour { get; set; }
        public bool Center { get; set; }

        public Text(params IElement[] children) : base(children)
        {
        }

        public Text(string value, params IElement[] children) : base(children)
        {
            Value = value;
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

            context.SpriteBatch.Begin();
            if (Center)
            {
                context.SpriteBatch.DrawString(font, Value, screenPosition.Location.ToVector2() - new Vector2(width, 0), Colour ?? Color.White);
            }
            else
            {
                context.SpriteBatch.DrawString(font, Value, screenPosition.Location.ToVector2(), Colour ?? Color.White);
            }
            context.SpriteBatch.End();
        }
    }
}
