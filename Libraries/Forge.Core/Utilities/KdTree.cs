using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Forge.Core.Utilities
{
    /// <summary>
    /// K-D tree data structure.
    /// </summary>
    public class KdTree<T>
    {
        internal KdTree(KdNode<T> root, Func<T, Vector3> locatorFunc)
        {
            Root = root;
            _locatorFunc = locatorFunc;
        }

        /// <summary>
        /// The root node.
        /// </summary>
        private KdNode<T> Root { get; }

        private Func<T, Vector3> _locatorFunc;

        /// <summary>
        /// Gets the node within the k-d tree for which the element's location corresponds.
        /// </summary>
        /// <param name="element">element</param>
        /// <returns></returns>
        public KdNode<T> GetCorrespondingNode(T element)
        {
            var location = _locatorFunc(element);

            return Decide(location, Root);
        }

        /// <summary>
        /// Decides whether a location is on the left or right of a node based on it's location for non-leaf nodes.
        /// For leaf nodes the leaf node itself is returned.
        /// </summary>
        /// <param name="location">world location</param>
        /// <param name="node">node</param>
        /// <returns></returns>
        private KdNode<T> Decide(Vector3 location, KdNode<T> node)
        {
            if (node.IsLeaf)
            {
                return node;
            }

            var value = KdNode<T>.GetDimension(node.Dimension, location);
            if (value > node.Threshold)
            {
                return Decide(location, node.Left);
            }
            else
            {
                return Decide(location, node.Right);
            }
        }

        /// <summary>
        /// Searches in a radius around an element.
        /// 
        /// Returns an enumerable of element around the search zone. This may include some which are outside of the radius.
        /// 
        /// Optionally performs an additional distance cull to remove elements outside of the radius.
        /// </summary>
        /// <param name="element">element</param>
        /// <param name="radius">radius</param>
        /// <param name="distanceCull">optional distance cull</param>
        /// <returns></returns>

        public IEnumerable<T> GetInRadius(T element, float radius, bool distanceCull = true)
        {
            var location = _locatorFunc(element);
            return GetInRadius(element, radius, distanceCull);
        }

        /// <summary>
        /// Searches in a radius around a location.
        /// 
        /// Returns an enumerable of element around the search zone. This may include some which are outside of the radius.
        /// 
        /// Optionally performs an additional distance cull to remove elements outside of the radius.
        /// </summary>
        /// <param name="location">location</param>
        /// <param name="radius">radius</param>
        /// <param name="distanceCull">optional distance cull</param>
        /// <returns></returns>

        public IEnumerable<T> GetInRadius(Vector3 location, float radius, bool distanceCull = true)
        {
            var candidates = new List<T>();
            // Gather candidates, starting recursively at the root node.
            GatherCandidates(location, Root, radius, candidates);

            // Additional filtering stage for ruling out elements outside of the radius.
            if (distanceCull)
            {
                var sqRad = radius * radius;
                var refinedCandidates = new List<T>();
                foreach (var el in candidates)
                {
                    var pos = _locatorFunc(el);
                    var dist = (pos - location).LengthSquared();
                    if (dist <= sqRad)
                    {
                        refinedCandidates.Add(el);
                    }
                }
                candidates = refinedCandidates;
            }

            return candidates;
        }

        /// <summary>
        /// Recursively gather candidates.
        /// </summary>
        /// <param name="location">location we are searching around</param>
        /// <param name="node">node</param>
        /// <param name="radius">radius in which we are searching</param>
        /// <param name="candidates"></param>
        private void GatherCandidates(Vector3 location, KdNode<T> node, float radius, IList<T> candidates)
        {
            // Leaf node: add all candiadtes.
            if (node.IsLeaf)
            {
                foreach (var element in node.LeafElements)
                {
                    candidates.Add(element);
                }
                return;
            }

            var value = KdNode<T>.GetDimension(node.Dimension, location);
            if (Math.Abs(value - node.Threshold) < radius)
            {
                // Case: we're within (radius) of the boundary, so check both sides.
                GatherCandidates(location, node.Left, radius, candidates);
                GatherCandidates(location, node.Right, radius, candidates);
            }
            else if (value > node.Threshold)
            {
                // Case: we're a safe distance on the left hand side.
                GatherCandidates(location, node.Left, radius, candidates);
            }
            else
            {
                // Case: we're a safe distance on the right hand side.
                GatherCandidates(location, node.Right, radius, candidates);
            }
        }

        public int Nodes => Root.Nodes;
    }
}
