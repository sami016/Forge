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
        private float? _height;

        public float Width => _width.Value;
        public float Height => _height.Value;

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
            _height = _width * (graphicsDevice.Viewport.Height / (float)graphicsDevice.Viewport.Width);

            if (!MinY.HasValue)
            {
                MinY = -_height.Value / 2;
            }
            if (!MaxY.HasValue)
            {
                MaxY = _height.Value / 2;
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

        public Ray CreateRay(Vector3 cameraLocation, Vector2 screenPos, Matrix inverseView, Matrix inverseViewNoTranslate)
        {
            var worldDirection = Vector3.Transform(Vector3.Forward, inverseView);
            worldDirection.Normalize();
            var worldSpaceUp = Vector3.Transform(Vector3.Up, inverseViewNoTranslate);
            var worldSpaceLeft = Vector3.Transform(Vector3.Left, inverseViewNoTranslate);

            Console.WriteLine(worldSpaceUp);
            var pos = cameraLocation + screenPos.X * worldSpaceLeft * _width.Value + screenPos.Y * worldSpaceUp * _height.Value;
            //Console.WriteLine(pos);

            return new Ray(pos, worldDirection);
        }
    }
}
