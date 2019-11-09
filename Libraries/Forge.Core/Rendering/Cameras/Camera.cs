using Forge.Core.Components;
using Forge.Core.Engine;
using Forge.Core.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Forge.Core.Rendering.Cameras
{
    public class Camera : Component, IInit
    {
        [Inject] public GraphicsDevice GraphicsDevice { get; set; }
        [Inject] public Transform Position { get; set; }

        private static Matrix _rotationOffset = Matrix.CreateRotationY(MathHelper.ToRadians(90));
        private static Matrix _rotationOffsetInverse = Matrix.CreateRotationY(MathHelper.ToRadians(-90));

        public Matrix Transform { get; set; }

        /* ICamera implementation */
        private Matrix _world;
        public Matrix View { get; private set; }
        public Matrix InverseView { get; private set; }
        public Matrix InverseViewNoTranslate { get; private set; }

        public Matrix Projection => CameraParameters.Projection;
        public Matrix InverseProjection => CameraParameters.InverseProjection;

        public IDictionary<string, object> Parameters { get; } = new Dictionary<string, object>();

        private ICameraParameters _cameraParameters;
        public ICameraParameters CameraParameters
        {
            get => _cameraParameters;
            set
            {
                _cameraParameters = value;
                if (GraphicsDevice != null)
                {
                    _cameraParameters.Recalculate(GraphicsDevice);
                }
            }
        }

        public Camera()
        {
        }

        public Camera(ICameraParameters cameraParameters)
        {
            CameraParameters = cameraParameters;
        }

        public void Initialise()
        {
            if (CameraParameters == null)
            {
                CameraParameters = new PerspectiveCameraParameters
                {
                    AspectRatio = GraphicsDevice.Adapter.CurrentDisplayMode.AspectRatio
                };
            }
            _cameraParameters.Recalculate(GraphicsDevice);
            Recalculate();
        }


        public Vector3 Forwards { get; private set; }
        public Vector3 Left { get; private set; }

        public Vector3 Up { get; private set; }

        public void RecalculateParameters()
        {
            CameraParameters?.Recalculate(GraphicsDevice);
        }

        public void Recalculate()
        {
            var basisTransform = Matrix.CreateFromQuaternion(Position.Rotation);
            // Calculate the camera's world matrix.
            _world = basisTransform * Matrix.CreateTranslation(Position.Location);
            // The view matrix is the inverse of the camera's world matrix.
            View = Matrix.Invert(_world);
            InverseView = _world;

            InverseViewNoTranslate = new Matrix(
                _world.M11, _world.M12, _world.M13, 0,
                _world.M21, _world.M22, _world.M23, 0,
                _world.M31, _world.M32, _world.M33, 0,
                _world.M41, _world.M42, _world.M43, _world.M44
            );


            Forwards = Vector3.Transform(
                Vector3.Forward,
                basisTransform
            );
            Up = Vector3.Transform(
                Vector3.Up,
                basisTransform
            );
            Left = Vector3.Transform(
                Vector3.Left,
                basisTransform
            );
        }

        public Vector3 ScreenToWorldDirection(Vector2 screenPos)
        {
            var screenPos4 = new Vector4(screenPos.X, screenPos.Y, 1, 1);

            var viewSpace = Vector4.Transform(screenPos4, InverseProjection);

            var worldTrasform = Vector4.Transform(viewSpace, InverseView);

            return new Vector3(worldTrasform.X, worldTrasform.Y, worldTrasform.Z);
            //return Vector3.Transform(screenPos3, InverseProjection);
        }


        public void Apply(Effect effect)
        {
            if (effect is IEffectMatrices)
            {
                var effectMatrices = effect as IEffectMatrices;
                effectMatrices.Projection = Projection;
                effectMatrices.View = View;
            }
        }

        public void LookAt(Vector3 location, Vector3? up = null)
        {
            var upDirection = up ?? Vector3.Up;

            var rotationMatrix = Matrix.CreateLookAt(Vector3.Zero, location - Position.Location, upDirection);
            Position.Rotation = Quaternion.Inverse(Quaternion.CreateFromRotationMatrix(rotationMatrix));
        }


        public void LookInDirection(Vector3 moveVector)
        {
            if (moveVector.LengthSquared() > 0)
            {
                LookAt(Position.Location + moveVector, Vector3.Up);
            }
        }
    }
}
