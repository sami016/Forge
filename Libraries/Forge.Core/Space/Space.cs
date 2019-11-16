using Forge.Core.Components;
using Forge.Core.Engine;
using Forge.Core.Interfaces;
using Forge.Core.Utilities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Forge.Core.Space
{
    /// <summary>
    /// An optimised space container that contains a set of positioned entities.
    /// </summary>
    public class Space : Component, ITick, ISpace
    {
        // Whether to force manual control of the space's entities.
        private readonly bool _manual;
        private readonly IList<Entity> _manualEntities = new List<Entity>();

        // Factory used to create kd trees.
        private readonly KdTreeFactory<Entity> _treeFactory;
        // The latest KD-tree of objects within the space.
        private KdTree<Entity> _tree;

        public Space(bool manual = false, int splitOver = 25, int dimensions = 2)
        {
            _manual = manual;
            _treeFactory = new KdTreeFactory<Entity>(splitOver, x => x.Get<Transform>().Location, dimensions);
        }

        public void Tick(TickContext context)
        {
            IEnumerable<Entity> positionables;
            if (!_manual)
            {
                positionables = Entity.EntityManager.GetAll<Transform>()
                    .Select(x => x.Entity)
                    .ToArray();
            } else
            {
                positionables = _manualEntities;
            }

            // Update the k-d tree to be used in the next tick.
            // Note: the k-d is approximate, since it is always calculated the tick before.
            // This optimisation is a reasonable trade off in most cases however, 
            // and at worst will lead to collisions being missed for 1 tick.
            var tree = _treeFactory.Create(positionables);
            this.Update(() =>
            {
                _tree = tree;
            });
        }

        public IEnumerable<Entity> GetNearby(Vector3 location, float radius)
        {
            if (_tree == null)
            {
                return new Entity[0];
            }
            return _tree?.GetInRadius(location, radius, true);
        }

        public void Add(Entity entity)
        {
            _manualEntities.Add(entity);
        }

        public void Remove(Entity entity)
        {
            _manualEntities.Remove(entity);
        }
    }
}
