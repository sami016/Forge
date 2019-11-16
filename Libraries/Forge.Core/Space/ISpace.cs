using Forge.Core.Engine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Forge.Core.Space
{
    public interface ISpace
    {
        IEnumerable<Entity> GetNearby(Vector3 location, float radius);

        /// <summary>
        /// Manually adds an entity to the space.
        /// </summary>
        /// <param name="entity">entity</param>
        void Add(Entity entity);

        /// <summary>
        /// Manually removes an entity from the space.
        /// </summary>
        /// <param name="entity">entity</param>
        void Remove(Entity entity);
    }
}
