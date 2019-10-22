using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Forge.UI.Glass.Elements
{
    /// <summary>
    /// Element primitive abstract implementation.
    /// </summary>
    public abstract class Primitive : IElement
    {
        public IList<IElement> Children { get; }

        public Primitive(IEnumerable<IElement> children)
        {
            Children = children.ToList();
        }

        public abstract void Render(RenderContext context);
    }
}
