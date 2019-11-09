using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Forge.Core.Space.Shapes
{
    public interface IShape3
    {
        /// <summary>
        /// The radius around the 0 point about which collisions may occur.
        /// If the shape is a sphere, this is the radius.
        /// </summary>
        float CollisionRadius { get; }

        /// <summary>
        /// Checks whether a point is inside the shape.
        /// </summary>
        /// <param name="point"></param>
        /// <param name="translation"></param>
        /// <returns></returns>
        bool IsInside(Vector3 point, Vector3 translation);

        /// <summary>
        /// Gets the center of a shape.
        /// </summary>
        /// <param name="translation">translation</param>
        /// <returns></returns>
        Vector3 GetCenter(Vector3 translation);

        /// <summary>
        /// Gets collisions of the shape with a ray.
        /// </summary>
        /// <param name="ray"></param>
        /// <returns></returns>
        Vector3[] GetCollisions(Ray ray, Matrix transform);

        /// <summary>
        /// Gets the closest collision (if any) with a ray.
        /// </summary>
        /// <param name="ray"></param>
        /// <returns></returns>
        Vector3? GetCollision(Ray ray, Matrix transform);
    }
}
