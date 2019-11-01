using Forge.Core.Components;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Forge.Core.Rendering
{
    public interface IRenderable : IComponent
    {
        void Render(RenderContext context);
    }
}
