using Forge.Core.Interfaces;
using Forge.UI.Glass.Interaction;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Forge.UI.Glass.Elements
{
    public interface IElement : IInit
    {
        IList<IElement> Children { get; }
        void Render(UIRenderContext context);
        Rectangle Position { get; set; }
        UIEvents Events { get; }
        Action<IElement> Init { get; set; }
    }
}
