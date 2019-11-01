using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Forge.Core.Rendering
{
    public class VertexBufferBuilder<T>
        where T : struct
    {
        private readonly T[] _data;
        private ushort _index = 0;

        public T[] Data => _data;

        public VertexBufferBuilder(int numberOfIndexes)
        {
            _data = new T[numberOfIndexes];
        }

        public VertexBufferBuilder<T> AddNext(T index)
        {

            _data[_index++] = index;
            return this;
        }

        public VertexBuffer Build(GraphicsDevice graphicsDevice)
        {
            var vertexBuffer = new VertexBuffer(graphicsDevice, typeof(T), _data.Length, BufferUsage.WriteOnly);
            vertexBuffer.SetData<T>(_data);
            return vertexBuffer;
        }
    }
}
