using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Forge.Core.Rendering.Cameras
{
    public interface ICameraParameters
    {
        Matrix Projection { get; }
        Matrix InverseProjection { get; }
        void Recalculate(GraphicsDevice graphicsDevice);
        Ray CreateRay(Vector3 cameraLocation, Vector2 screenPos, Matrix inverseView, Matrix inverseViewNoTranslate);
    }
}
