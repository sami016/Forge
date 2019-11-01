using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Forge.Core.Rendering.VertexTypes
{
    // Corresponds with VS_TextureOnly.
    public struct VertexPositionNormal : IVertexType
    {
        private static readonly VertexDeclaration VertexDeclarationValue = new VertexDeclaration(
            24,
            new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
            new VertexElement(12, VertexElementFormat.Vector3, VertexElementUsage.Normal, 0)
        );

        public VertexDeclaration VertexDeclaration => VertexDeclarationValue;

        public Vector3 Position { get; set; }
        public Vector3 Normal { get; set; }

        public VertexPositionNormal(Vector3 position, Vector3 normal)
        {
            Position = position;
            Normal = normal;
        }

    }
}
