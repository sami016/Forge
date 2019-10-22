using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Forge.UI.Glass.Elements
{
    public class Text : Primitive
    {
        public string Value { get; set; }

        public Text(params IElement[] children) : base(children)
        {
        }

        public override void Render(RenderContext context)
        {
            context.SpriteBatch.Begin();
            context.SpriteBatch.DrawString(context.Fonts.Get("Default"), Value, new Vector2(10, 10), Color.Black);
            context.SpriteBatch.End();
        }
    }
}
