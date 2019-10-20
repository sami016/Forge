using Forge.Core.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace Forge.Core.Engine
{
    /// <summary>
    /// Update static utility class for setting updates.
    /// </summary>
    public static class UpdateExtensions
    {
        /// <summary>
        /// Applies an update at the end of this tick.
        /// </summary>
        /// <param name="updateAction"></param>
        public static void Update(this EntityManager entityManager, Action updateAction)
        {
            entityManager.UpdateContext.Enqueue(0, updateAction);
        }

        /// <summary>
        /// Applies an update at the end of this tick.
        /// </summary>
        /// <param name="updateAction"></param>
        public static void Update(this Entity entity, Action updateAction)
        {
            entity.EntityManager.UpdateContext.Enqueue(entity.ThreadContextNumber, updateAction);
        }

        /// <summary>
        /// Applies an update at the end of this tick.
        /// </summary>
        /// <param name="updateAction"></param>
        public static void Update(this IComponent component, Action updateAction)
        {
            var entity = component.Entity;
            entity.EntityManager.UpdateContext.Enqueue(entity.ThreadContextNumber, updateAction);
        }
    }
}
