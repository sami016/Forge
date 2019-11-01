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

        public Text(params IElement[] children) : base(children)
        {
        }

        public override void Render(UIRenderContext context)
        {
            var font = context.Fonts.Get(Font ?? "Default");
            if (font == null)
            {
                font = context.Fonts.Get("Default");
            }
            context.SpriteBatch.Begin();
            context.SpriteBatch.DrawString(font, Value, new Vector2(10, 10), Color.White);
            context.SpriteBatch.End();
        }
    }
}
