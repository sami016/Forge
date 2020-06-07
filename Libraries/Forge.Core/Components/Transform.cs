using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Forge.Core.Components
{
    /// <summary>
    /// Used to position entities within the world.
    /// </summary>
    public class Transform : Component
    {
        private bool _dirty = true;

        private Vector3 _location = Vector3.Zero;
        /// <summary>
        /// The position of the object.
        /// </summary>
        public Vector3 Location
        {
            get => _location;
            set
            {
                _location = value;
                _dirty = true;
            }
        }


        private Vector3 _scale = Vector3.One;
        /// <summary>
        /// The position of the object.
        /// </summary>
        public Vector3 Scale
        {
            get => _scale;
            set
            {
                _scale = value;
                _dirty = true;
            }
        }

        private Quaternion _rotation = Quaternion.Identity;
        /// <summary>
        /// The rotation of the object.
        /// </summary>
        public Quaternion Rotation
        {
            get => _rotation;
            set
            {
                _rotation = value;
                _dirty = true;
            }
        }

        private Vector3 _velocity = Vector3.Zero;
        /// <summary>
        /// The velocity of the object.
        /// </summary>
        public Vector3 Velocity
        {
            get => _velocity;
            set
            {
                _velocity = value;
                _dirty = true;
            }
        }
        
        private Vector3? _rotationCenter = null;
        /// <summary>
        /// The velocity of the object.
        /// </summary>
        public Vector3 RotationCenter
        {
            get => _rotationCenter ?? Vector3.Zero;
            set
            {
                _rotationCenter = value;
                _dirty = true;
            }
        }

        private Matrix _worldTransform;
        private Matrix? _parentTransformLast;
        private readonly Transform _parent;

        public Transform Parent => _parent;

        /// <summary>
        /// The world transform of the object.
        /// </summary>
        public Matrix WorldTransform
        {
            get
            {
                // Update the transform in a lazy manner whenever state is dirty.
                if (
                    _dirty
                    || !_parentTransformLast.HasValue
                    || _parentTransformLast.Value != _parent.WorldTransform
                )
                { 
                    _worldTransform = Matrix.Identity;
                    _worldTransform *= Matrix.CreateScale(Scale);
                    if (_rotationCenter.HasValue)
                    {
                        _worldTransform *= Matrix.CreateTranslation(-_rotationCenter.Value);
                        _worldTransform *= Matrix.CreateFromQuaternion(_rotation);
                        _worldTransform *= Matrix.CreateTranslation(_rotationCenter.Value);
                    } 
                    else
                    {
                        _worldTransform *= Matrix.CreateFromQuaternion(_rotation);
                    }
                    _worldTransform *= Matrix.CreateTranslation(Location);
                    if (_parent != null)
                    {
                        _parentTransformLast = _parent.WorldTransform;
                        _worldTransform *= _parentTransformLast.Value;
                    }
                    _dirty = false;
                }
                return _worldTransform;
            }
        }

        public Transform()
        {

        }

        public Transform(Transform parent)
        {
            _parent = parent;
        }

        public Transform(Vector3 position, Transform parent = null): this(parent)
        {
            Location = position;
        }

        public Vector3 GlobalLocation
        {
            get
            {
                if (_parent == null)
                {
                    return Location;
                }
                return Vector3.Transform(Location, _parent.WorldTransform);
            }
        }
    }
}
