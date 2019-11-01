using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Forge.Core.Rendering
{
    public class IndexBufferBuilder<T>
        where T : struct
    {
        private readonly T[] _data;
        private ushort _index = 0;

        public T[] Data => _data;

        public IndexBufferBuilder(int numberOfIndexes)
        {
            _data = new T[numberOfIndexes];
        }

        public IndexBufferBuilder<T> AddNext(T index)
        {

            _data[_index++] = index;
            return this;
        }


        public IndexBufferBuilder<T> AddNext(T indexA, T indexB, T indexC)
        {

            _data[_index++] = indexA;
            _data[_index++] = indexB;
            _data[_index++] = indexC;
            return this;
        }



        public IndexBuffer Build(GraphicsDevice graphicsDevice)
        {
            var indexBuffer = new IndexBuffer(graphicsDevice, IndexElementSize.SixteenBits, _data.Length,
                BufferUsage.WriteOnly);
            indexBuffer.SetData<T>(_data);
            return indexBuffer;
        }
    }
}
