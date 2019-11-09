using System;
using System.Collections.Generic;
using System.Text;

namespace Forge.Core.Space.Collisions
{
    using Microsoft.Xna.Framework;
    using System;
    using System.Collections.Generic;
    using System.Text;

    namespace Ingot.Engine.Physics.Collisions
    {
        public static class CollisionMath
        {
            private static float EPSILON = 0.0001f;

            public static Vector3 Test(Vector3 v0, Vector3 v1)
            {
                if ((v1-v0).LengthSquared() <= 0)
                {
                    throw new Exception("This should not happen");
                }
                return v1 - v0;
            }

            /// <summary>
            /// 
            /// see: https://en.wikipedia.org/wiki/M%C3%B6ller%E2%80%93Trumbore_intersection_algorithm
            /// </summary>
            /// <param name="ray"></param>
            /// <param name="a"></param>
            /// <param name="b"></param>
            /// <param name="c"></param>
            /// <returns></returns>
            public static (bool, Vector3) CollideTri(Ray ray, Vector3 v0, Vector3 v1, Vector3 v2)
            {
                var edge1 = v1 - v0;
                var edge2 = v2 - v0;

                // Invalid face detected.
                if (edge1.LengthSquared() <= 0 || edge2.LengthSquared() <= 0)
                {
                    return (false, Vector3.Zero);
                }
                var rayEdge2Normal = Vector3.Cross(ray.Direction, edge2);
                var a = Vector3.Dot(edge1, rayEdge2Normal);
                if (a > -EPSILON && a < EPSILON)
                {
                    return (false, Vector3.Zero);    // This ray is parallel to this triangle.
                }
                var f = 1.0 / a;
                var s = ray.Position - v0;
                var u = f * Vector3.Dot(s, rayEdge2Normal);
                if (u < 0.0 || u > 1.0)
                {
                    return (false, Vector3.Zero);
                }
                var q = Vector3.Cross(s, edge1);
                var v = f * Vector3.Dot(ray.Direction, q);
                if (v < 0.0 || u + v > 1.0)
                {
                    return (false, Vector3.Zero);
                }
                // At this stage we can compute t to find out where the intersection point is on the line.
                var t = f * Vector3.Dot(edge2, q);
                if (t > EPSILON) // ray intersection
                {
                    return (true, ray.Position + ray.Direction * (float)t);
                }
                else // This means that there is a line intersection but not a ray intersection.
                {
                    return (false, Vector3.Zero);
                }
            }
        }
    }
}
