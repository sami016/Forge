using Forge.Core.Components;
using Forge.Core.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Forge.Terrain
{
    public class TerrainMeshHelpers : Component
    {

        [Inject] public GraphicsDevice GraphicsDevice { get; set; }

        public GpuData CreateTerrainMesh(Bitmap[,] bitmap)
        {
            if (bitmap.GetLength(0) != 3
                || bitmap.GetLength(1) != 3)
            {
                throw new Exception("Expected input bitmaps set to be of shape 3 by 3");
            }

            var imageWidth = bitmap[1,1].Width;
            var imageHeight = bitmap[1,1].Height;
            var meshWidth = imageWidth + 1;
            var meshHeight = imageHeight + 1;

            var indexBufferBuilder = new IndexBufferBuilder<ushort>(3 * 2 * imageHeight * imageWidth);
            var vertexBufferBuilder = new VertexBufferBuilder<VertexPositionNormalTexture>(meshWidth * meshHeight);

            var random = new Random();
            // Define sample function.
            float Sample(int coordinateX, int coordinateZ)
            {
                return (float)random.NextDouble();
            }

            // Build vertices.
            var debugIndex = 0;
            for (var x=0; x<meshWidth; x++)
            {
                for (var z=0; z<meshHeight; z++)
                {
                    var rx = (x / (float)(meshWidth-1));
                    var rz = (z / (float)(meshHeight-1));

                    // v0_0 ◀---▶ v1_0
                    //  ▲            ▲
                    //  |      x     |
                    //  ▼            ▼
                    // v0_1 ◀---▶ v1_1

                    var v0_0 = Sample(x, z);
                    var v0_1 = Sample(x, z+1);
                    var v1_0 = Sample(x+1, z);
                    var v1_1 = Sample(x+1, z+1);

                    // Sample the height as the average of the four surrounding pixels.
                    var height = (v0_0 + v0_1 + v1_0 + v1_1) / 4f;

                    var wz = 1;
                    var wx = 0;
                    var wy = v0_1 - v0_0;
                    var vx = 1;
                    var vz = 0;
                    var vy = v1_0 - v0_0;

                    var normal_x = (vy * wz) - (vz * wy);
                    var normal_y = (vz * wx) - (vx * wz);
                    var normal_z = (vx * wy) - (vy * wx);

                    var normal = new Vector3(normal_x, normal_y, normal_z);
                    if (normal.Y < 0)
                    {
                        normal = -normal;
                    }
                    normal.Normalize();

                    Console.WriteLine($"index: {debugIndex} - rx: {rx}   rz: {rz}");
                    debugIndex++;
                    vertexBufferBuilder.AddNext(
                        new VertexPositionNormalTexture(
                            new Vector3(rx, height, rz),
                            normal,
                            new Vector2(rx, rz)
                        )
                    );
                }
            }

            ushort GetVertexIndex(int x, int z)
            {
                return (ushort)(x + z * meshWidth);
            }

            for (var x=0; x<imageWidth; x++)
            {
                for (var z=0; z<imageHeight; z++)
                {
                    var i0_0 = GetVertexIndex(x, z);
                    var i1_0 = GetVertexIndex(x+1, z);
                    var i0_1 = GetVertexIndex(x, z+1);
                    var i1_1 = GetVertexIndex(x+1, z+1);

                    Console.WriteLine($"tri {i0_0} {i1_0} {i0_1}");
                    Console.WriteLine($"tri {i1_0} {i1_1} {i0_1}");

                    indexBufferBuilder
                           .AddNext(i0_0, i0_1, i1_0)
                           .AddNext(i1_0, i0_1, i1_1);
                }
            }

            return GpuData.Create(indexBufferBuilder, vertexBufferBuilder, GraphicsDevice);
        }
    }
}
