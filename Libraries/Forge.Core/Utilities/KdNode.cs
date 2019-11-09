using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Forge.Core.Utilities
{
    public class KdNode<T>
    {
        public KdNode(byte dimension, float threshold, KdNode<T> left, KdNode<T> right)
        {
            Dimension = dimension;
            Threshold = threshold;
            Left = left;
            Right = right;
        }

        public KdNode(IEnumerable<T> leafElements)
        {
            Dimension = 255;
            LeafElements = leafElements;
        }

        /// <summary>
        /// The dimension on which the node is filtering in.
        /// 
        /// Set to 255 if the node is a leaf node.
        /// </summary>
        public byte Dimension { get; set; } = 0;

        /// <summary>
        /// The threshold on which to base the decision.
        /// 
        /// Check v(Dimension) > Threshold
        /// => Left subtree
        /// Else => Right subtree
        /// 
        /// </summary>
        public float Threshold { get; set; } = 0;

        /// <summary>
        /// Left child node, this is for positions than exceed the threshold in the relevant dimension.
        /// </summary>
        public KdNode<T> Left { get; set; } = null;

        /// <summary>
        /// Right child node, this is for positions that are less than or equal to the threshold in the relevant dimension.
        /// </summary>
        public KdNode<T> Right { get; set; } = null;

        /// <summary>
        /// Enumeration of the items, set only on the leaf nodes.
        /// </summary>
        public IEnumerable<T> LeafElements { get; set; } = null;


        /// <summary>
        /// Gets the number of nodes in subtree.
        /// </summary>
        public int Nodes
        {
            get
            {
                if (IsLeaf)
                {
                    return 1;
                }
                return 1 + Left.Nodes + Right.Nodes;
            }
        }

        public bool IsLeaf => Dimension == 255;

        public static float GetDimension(int dimension, Vector3 vec)
        {
            if (dimension == 0)
            {
                return vec.X;
            }
            if (dimension == 1)
            {
                return vec.Y;
            }
            if (dimension == 2)
            {
                return vec.Z;
            }
            return -1;
        }
    }
}
