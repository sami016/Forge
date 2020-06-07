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
        [Inject] public Transform Transform { get; set; }

        private static Matrix _rotationOffset = Matrix.CreateRotationY(MathHelper.ToRadians(90));
        private static Matrix _rotationOffsetInverse = Matrix.CreateRotationY(MathHelper.ToRadians(-90));

        //public Matrix TransformMatrix { get; set; }

        /* ICamera implementation */
        public Matrix World { get; private set; }
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

        public Ray CreateRay(Vector2 screenPos)
        {
            return CameraParameters.CreateRay(Transform.Location, screenPos, InverseView, InverseViewNoTranslate);
        }

        public void Recalculate()
        {
            var basisTransform = Matrix.CreateFromQuaternion(Transform.Rotation);
            // Calculate the camera's world matrix.
            World = basisTransform * Matrix.CreateTranslation(Transform.Location);
            // The view matrix is the inverse of the camera's world matrix.
            View = Matrix.Invert(World);
            InverseView = World;

            InverseViewNoTranslate = new Matrix(
                World.M11, World.M12, World.M13, 0,
                World.M21, World.M22, World.M23, 0,
                World.M31, World.M32, World.M33, 0,
                World.M41, World.M42, World.M43, World.M44
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

            var rotationMatrix = Matrix.CreateLookAt(Vector3.Zero, location - Transform.Location, upDirection);
            Transform.Rotation = Quaternion.Inverse(Quaternion.CreateFromRotationMatrix(rotationMatrix));
        }


        public void LookInDirection(Vector3 moveVector)
        {
            if (moveVector.LengthSquared() > 0)
            {
                LookAt(Transform.Location + moveVector, Vector3.Up);
            }
        }
    }
}
