using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Forge.Core.Utilities
{
    public class KdTreeFactory<T>
    {
        private int _splitOver;
        private Func<T, Vector3> _locatorFunc;
        private int _dimensions;

        public KdTreeFactory(int splitOver, Func<T, Vector3> locatorFunc, int dimensions = 2)
        {
            _splitOver = splitOver;
            _locatorFunc = locatorFunc;
            _dimensions = dimensions;
        }


        public KdTree<T> Create(IEnumerable<T> elements)
        {
            return new KdTree<T>(CreateNode(elements), _locatorFunc);
        }

        /// <summary>
        /// Creates a node from a set of elements.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="splitOver"></param>
        /// <param name="elements"></param>
        /// <param name="locatorFunction"></param>
        /// <returns></returns>
        private KdNode<T> CreateNode(IEnumerable<T> elements, byte dimension = 0)
        {
            if (elements.Count() <= _splitOver)
            {
                return new KdNode<T>(elements);
            }

            var positions = elements.Select(_locatorFunc);
            var values = positions.Select(x => KdNode<T>.GetDimension(dimension, x));
            var threshold = GetCenter(values);

            var leftElements = new List<T>();
            var rightElements = new List<T>();

            // Sort elements into left and right elements.
            foreach (var el in elements)
            {
                var value = KdNode<T>.GetDimension(dimension, _locatorFunc(el));
                if (value > threshold)
                {
                    leftElements.Add(el);
                }
                else
                {
                    rightElements.Add(el);
                }
            }

            // To prevent a bug occuring when all items are in the same position.
            if (leftElements.Count() == 0)
            {
                return new KdNode<T>(rightElements);
            }
            if (rightElements.Count() == 0)
            {
                return new KdNode<T>(leftElements);
            } 

            var nextDimension = (byte)((dimension + 1) % _dimensions);

            return new KdNode<T>(
                dimension,
                threshold,
                CreateNode(leftElements, nextDimension),
                CreateNode(rightElements, nextDimension)
            );
        }

        private static float GetCenter(IEnumerable<float> locations)
        {
            var acc = 0f;
            foreach (var location in locations)
            {
                acc += location;
            }
            return acc / locations.Count();
        }


    }
}
