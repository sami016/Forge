using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Forge.Core.Rendering
{
    public struct GpuData
    {

        public ushort[] Indices;
        public IEnumerable Vertices;


        public static GpuData Create<T>(IndexBufferBuilder<ushort> indexBufferBuilder, VertexBufferBuilder<T> vertexBufferBuilder, GraphicsDevice graphicsDevice)
            where T : struct
        {
            return new GpuData
            {
                IndexBuffer = graphicsDevice == null ? null : indexBufferBuilder.Build(graphicsDevice),
                VertexBuffer = graphicsDevice == null ? null : vertexBufferBuilder.Build(graphicsDevice),

                Indices = indexBufferBuilder.Data,
                Vertices = vertexBufferBuilder.Data
            };
        }

        public static GpuData CreateTesting<T>(ushort[] indices, T[] verts)
        {
            return new GpuData()
            {
                Indices = indices,
                Vertices = verts,
            };
        }

        public IndexBuffer IndexBuffer;
        public VertexBuffer VertexBuffer;

        [Conditional("DEBUG")]
        public void TestPrintPositions(Matrix modelTransform, Matrix viewTransform, Matrix projectionTransform)
        {
            var array = Vertices.Cast<object>().ToArray();
            var indices = Indices;

            string formatIndex(int pos)
            {

                var index = indices[pos];
                var vertex = array[index];
                var coordinates = (Vector3)vertex.GetType().GetFields().FirstOrDefault(x => x.Name == "Position").GetValue(vertex);
                coordinates = Vector3.Transform(
                    Vector3.Transform(Vector3.Transform(coordinates, modelTransform), viewTransform),
                    projectionTransform);
                return coordinates.ToString();
            }

            //for (var i = 0; i < Indices.Length; i += 3)
            //{
            //    Console.WriteLine($"{formatIndex(i)} -> {formatIndex(i + 1)} -> {formatIndex(i + 2)}");
            //}
        }
    }

    public static class IndexVertexBufferExtensions
    {
        public static void RenderTriangleList(this GpuData indexVertexBuffer, GraphicsDevice graphicsDevice, int baseVertex = 0, int startIndex = 0)
        {
            graphicsDevice.SetVertexBuffer(indexVertexBuffer.VertexBuffer);
            graphicsDevice.Indices = indexVertexBuffer.IndexBuffer;
            graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, baseVertex, startIndex, indexVertexBuffer.IndexBuffer.IndexCount);
        }
    }
}
