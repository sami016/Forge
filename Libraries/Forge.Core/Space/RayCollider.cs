using Forge.Core.Components;
using Forge.Core.Engine;
using Forge.Core.Space.Bodies;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Forge.Core.Space
{
    /// <summary>
    /// Class for performing ray collisions.
    /// Should be attached to the entity with the space component.
    /// </summary>
    public class RayCollider : Component
    {
        [Inject] ISpace Space { get; set; }
        // Global entity manager.
        [Inject] EntityManager EntityManager { get; set; }


        public (Entity entity, Vector3 location) RayCast(Ray ray, byte? layerMask = null)
        {
            var withBodies = EntityManager.GetAll<Body>();
            var all = EntityManager.GetAll<Body>();
            if (layerMask.HasValue)
            {
                all = all.Where(x => (x.Layer & layerMask) > 0);
            }
            foreach (var body in all)
            {
                var position = body.Entity.Get<Transform>();
                if (position == null)
                {
                    continue;
                }
                //Console.WriteLine(position.Location);
                var hitLocation = body.Shape.GetCollision(ray, position.WorldTransform);
                if (hitLocation.HasValue)
                {
                    return (body.Entity, hitLocation.Value);
                }
            }
            return (null, Vector3.Zero);
        }
    }
}
