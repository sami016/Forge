using Forge.Core.Components;
using Forge.Core.Interfaces;
using Forge.Core.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace Forge.Core.Debugging
{
    public class FpsDebugger : Component, ITick, IRenderable
    {
        public uint RenderOrder { get; } = 0;
        public bool AutoRender { get; } = true;

        public void Render(RenderContext context)
        {
            var fps = 1f / context.GameTime.ElapsedGameTime.TotalSeconds;
            Console.WriteLine($"fps: {fps}");
        }

        public void Tick(TickContext context)
        {
            var tps = 1f / context.DeltaTimeSeconds;
            Console.WriteLine($"tps: {tps}");   
        }
    }
}
