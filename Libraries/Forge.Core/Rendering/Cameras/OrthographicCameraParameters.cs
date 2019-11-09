using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Forge.Core.Rendering.Cameras
{
    public class OrthographicCameraParameters : ICameraParameters
    {
        public Matrix Projection { get; private set; }
        public Matrix InverseProjection { get; private set; }
        public float ZNearPlane { get; set; } = 0.0001f;
        public float ZFarPlane { get; set; } = 1000f;
        public float? MinX { get; set; }
        public float? MinY { get; set; }
        public float? MaxX { get; set; }
        public float? MaxY { get; set; }
        private float? _width = null;

        public OrthographicCameraParameters()
        {
        }

        public OrthographicCameraParameters(float width)
        {
            _width = width;
        }

        public void Recalculate(GraphicsDevice graphicsDevice)
        {
            if (!_width.HasValue)
            {
                _width = graphicsDevice.Viewport.Width;
            }
            if (!MinX.HasValue)
            {
                MinX = -_width / 2;
            }
            if (!MaxX.HasValue)
            {
                MaxX = _width / 2;
            }
            var height = _width * (graphicsDevice.Viewport.Height / (float)graphicsDevice.Viewport.Width);

            if (!MinY.HasValue)
            {
                MinY = -height / 2;
            }
            if (!MaxY.HasValue)
            {
                MaxY = height / 2;
            }
            Projection = Matrix.CreateOrthographicOffCenter(
                MinX.Value,
                MaxX.Value,
                MinY.Value,
                MaxY.Value,
                ZNearPlane,
                ZFarPlane
            );
            InverseProjection = Matrix.Invert(Projection);
        }
    }
}
