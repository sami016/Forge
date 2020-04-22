using Forge.Core.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace Forge.Core.Rendering.Cameras
{
    public class CameraManager : Component, IRenderModifier
    {
        public Camera ActiveCamera { get; set; }

        public uint ApplicationOrder { get; } = 10;

        public void Apply(RenderContext context)
        {
            if (ActiveCamera != null)
            {
                context.Projection = ActiveCamera.Projection;
                context.View = ActiveCamera.View;
            }
        }
    }
}
