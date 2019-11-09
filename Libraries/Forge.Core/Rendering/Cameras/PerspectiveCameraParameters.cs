using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Forge.Core.Rendering.Cameras
{
    public class PerspectiveCameraParameters : ICameraParameters
    {
        public float AspectRatio { get; set; } = 1f;
        public float NearPlaneDistance { get; set; } = 0.01f;
        public float FarPlainDistance { get; set; } = 10000f;
        public float FoV { get; set; } = MathHelper.ToRadians(45f);

        public PerspectiveCameraParameters()
        {
        }

        public void Recalculate(GraphicsDevice graphicsDevice)
        {
            Projection = Matrix.CreatePerspectiveFieldOfView(
                FoV,
                AspectRatio,
                NearPlaneDistance,
                FarPlainDistance
            );
            InverseProjection = Matrix.Invert(Projection);
        }

        public Matrix Projection { get; private set; }
        public Matrix InverseProjection { get; private set; }
    }
}
