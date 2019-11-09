using Forge.Core.Space.Collisions.Ingot.Engine.Physics.Collisions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace Forge.Core.Space.Shapes
{
    public class MeshShape : IShape3
    {
        public Vector3[][] _faces;

        public float CollisionRadius { get; } = 0;

        public MeshShape(Vector3[][] faces)
        {
            _faces = faces;
            CollisionRadius = CalculateCollisionRadius();
        }

        static byte[] GetStructBytes<T>(T str)
            where T : struct
        {
            int size = Marshal.SizeOf(str);
            byte[] arr = new byte[size];

            IntPtr ptr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(str, ptr, true);
            Marshal.Copy(ptr, arr, 0, size);
            Marshal.FreeHGlobal(ptr);
            return arr;
        }

        static Vector3 ReadElementVector3(byte[] data, int index, VertexDeclaration vertexDeclaration, VertexElement positionElement)
        {
            if (positionElement.VertexElementFormat != VertexElementFormat.Vector3)
            {
                throw new Exception($"Expected vector3 but found {positionElement.VertexElementFormat}");
            }
            var vertexOffset = index * vertexDeclaration.VertexStride;
            //Console.WriteLine($"\tRead {index}[0] - {vertexOffset + positionElement.Offset}");
            var v0 = BitConverter.ToSingle(data, vertexOffset + positionElement.Offset);
            //Console.WriteLine($"\tRead {index}[1] - {vertexOffset + positionElement.Offset + 4}");
            var v1 = BitConverter.ToSingle(data, vertexOffset + positionElement.Offset + 4);
            //Console.WriteLine($"\tRead {index}[2] - {vertexOffset + positionElement.Offset + 8}");
            var v2 = BitConverter.ToSingle(data, vertexOffset + positionElement.Offset + 8);
            return new Vector3(v0, v1, v2);
        }

        static Vector3 GetPosition<TVertexType>(VertexElement positionElement, TVertexType vertex)
            where TVertexType : struct, IVertexType
        {
            var bytes = GetStructBytes<TVertexType>(vertex);

            var v0 = BitConverter.ToSingle(bytes, positionElement.Offset);
            var v1 = BitConverter.ToSingle(bytes, positionElement.Offset + 4);
            var v2 = BitConverter.ToSingle(bytes, positionElement.Offset + 8);

            return new Vector3(v0, v1, v2);
        }


        public static MeshShape FromModel(Model model)
        {

            var numVerts = 0;
            foreach (var mesh in model.Meshes)
            {
                foreach (var meshPart in mesh.MeshParts)
                {
                    numVerts += meshPart.IndexBuffer.IndexCount / 3;
                }
            }

            var faces = new Vector3[numVerts][];
            var i = 0;
            var meshCount = 0;
            foreach (var mesh in model.Meshes)
            {
                foreach (var meshPart in mesh.MeshParts)
                {
                    var declaration = meshPart.VertexBuffer.VertexDeclaration;
                    var elements = declaration.GetVertexElements();
                    var pos = elements.Where(x => x.VertexElementUsage == VertexElementUsage.Position).FirstOrDefault();

                    if (pos == null)
                    {
                        throw new Exception("No position in vertex data");
                    }

                    //Console.WriteLine("stride: "+declaration.VertexStride);
                    //Console.WriteLine($"mesh part {meshCount}");
                    //Console.WriteLine();
                    //Console.WriteLine("Raw data");
                    //Console.WriteLine();

                    var vertexData = new byte[meshPart.VertexBuffer.VertexCount * declaration.VertexStride];
                    meshPart.VertexBuffer.GetData<byte>(vertexData);
                    var indices = new ushort[meshPart.IndexBuffer.IndexCount];
                    meshPart.IndexBuffer.GetData<ushort>(indices);

                    for (var k = 0; k < vertexData.Length / 4; k++)
                    {
                        var single = BitConverter.ToSingle(vertexData, k * 4);
                        //Console.WriteLine($"[{k}] ({k*4}-{k*4+3}) = {single}");
                    }

                    //Console.WriteLine();
                    //Console.WriteLine("Faces");
                    //Console.WriteLine();

                    for (var k = 0; k < indices.Length; k += 3)
                    {
                        faces[i] = new Vector3[3];
                        faces[i][0] = ReadElementVector3(vertexData, indices[k], declaration, pos);
                        faces[i][1] = ReadElementVector3(vertexData, indices[k + 1], declaration, pos);
                        faces[i][2] = ReadElementVector3(vertexData, indices[k + 2], declaration, pos);

                        //Console.WriteLine($"[{indices[k]}]={faces[i][0]}, [{indices[k + 1]}]={faces[i][1]}, [{indices[k + 2]}]={faces[i][2]}");

                        i++;
                    }
                    meshCount++;
                }
            }

            return new MeshShape(faces);
        }

        public static MeshShape FromModel<TVertexType>(Model model)
            where TVertexType : struct, IVertexType
        {
            var vertexTypeInstance = new TVertexType();
            var stride = vertexTypeInstance.VertexDeclaration.VertexStride;
            var positionElement = vertexTypeInstance.VertexDeclaration.GetVertexElements()
                .Where(x => x.VertexElementFormat == VertexElementFormat.Vector3)
                .Where(x => x.VertexElementUsage == VertexElementUsage.Position)
                .FirstOrDefault();

            var numVerts = 0;
            foreach (var mesh in model.Meshes)
            {
                foreach (var meshPart in mesh.MeshParts)
                {
                    numVerts += meshPart.IndexBuffer.IndexCount / 3;
                }
            }

            var faces = new Vector3[numVerts][];
            var i = 0;
            foreach (var mesh in model.Meshes)
            {
                foreach (var meshPart in mesh.MeshParts)
                {
                    //var verts = new TVertexType[meshPart.VertexBuffer.VertexCount];
                    //var indices = new ushort[meshPart.IndexBuffer.IndexCount];
                    //meshPart.IndexBuffer.GetData<ushort>(indices);
                    //meshPart.VertexBuffer.GetData<TVertexType>(verts);

                    //for (var k = 0; k < indices.Length; k += 3)
                    //{
                    //    faces[i] = new Vector3[3];
                    //    faces[i][0] = GetPosition(positionElement, verts[indices[k]]);
                    //    faces[i][1] = GetPosition(positionElement, verts[indices[k + 1]]);
                    //    faces[i][2] = GetPosition(positionElement, verts[indices[k + 2]]);

                    //    //Console.WriteLine($"{faces[i][0]}, {faces[i][1]}, {faces[i][2]}");

                    //    i++;
                    //}
                }
            }

            return new MeshShape(faces);
        }

        private float CalculateCollisionRadius()
        {

            var sqDistance = 0.0;
            foreach (var face in _faces)
            {
                foreach (var vertex in face)
                {
                    sqDistance = Math.Max(sqDistance, vertex.LengthSquared());
                }
            }
            return (float)Math.Sqrt(sqDistance);
        }

        public Vector3 GetCenter(Vector3 translation)
        {
            return translation;
        }

        public Vector3[] GetCollisions(Ray ray, Matrix transform)
        {
            var results = new List<Vector3>(1);
            foreach (var face in _faces)
            {
                var v0 = Vector3.Transform(face[0], transform);
                var v1 = Vector3.Transform(face[1], transform);
                var v2 = Vector3.Transform(face[2], transform);
                var (hit, pos) = CollisionMath.CollideTri(
                    ray,
                    v0,
                    v1,
                    v2
                );
                //Console.WriteLine($"Faces: ({face[0]}), ({face[1]}), ({face[2]})");
                //Console.WriteLine($"Pos: {ray.Position}  Direction: {ray.Direction}");
                if (hit)
                {
                    //Console.WriteLine($"hit {v0} {v1} {v2}       {pos}");

                    results.Add(pos);
                }
            }
            return results.ToArray();
        }

        public Vector3? GetCollision(Ray ray, Matrix transform)
        {
            float lowestDistanceSoFar = float.MaxValue;
            Vector3? closest = null;

            foreach (var point in GetCollisions(ray, transform))
            {
                var pointDistance = (point - ray.Position).LengthSquared();
                if (pointDistance < lowestDistanceSoFar)
                {
                    lowestDistanceSoFar = pointDistance;
                    closest = point;
                }
            }

            return closest;
        }

        public bool IsInside(Vector3 point, Vector3 translation)
        {
            throw new NotImplementedException();
        }
    }
}
