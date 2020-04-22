using Forge.Core.Components;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Forge.Core.Rendering
{
    public interface IRenderModifier : IComponent
    {
        uint ApplicationOrder { get; }
        void Apply(RenderContext context);
    }
}
