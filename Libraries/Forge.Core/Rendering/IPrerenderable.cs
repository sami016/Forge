using Forge.Core.Components;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Forge.Core.Rendering
{
    /// <summary>
    /// A component for rendering things that need to be rendered before the main rendering begins.
    /// e.g. to use another render target.
    /// </summary>
    public interface IPrerenderable
    {
        uint RenderOrder { get; }
        bool AutoRender { get; }
        void Prerender(RenderContext context);
    }
}
