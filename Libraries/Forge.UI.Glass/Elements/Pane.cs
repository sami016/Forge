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
        public Pane(params IElement[] children) : base(children)
        {
        }

        public override void Render(RenderContext context)
        {
            foreach (var child in Children)
            {
                child.Render(context);
            }
        }
    }
}
