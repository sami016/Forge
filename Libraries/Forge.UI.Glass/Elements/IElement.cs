using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Forge.UI.Glass.Elements
{
    public interface IElement
    {
        IList<IElement> Children { get; }
        void Render(RenderContext context);
    }
}
