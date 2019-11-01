using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Forge.Core.Rendering.VertexTypes
{
    // Corresponds with VS_TextureOnly.
    public struct VertexPositionNormalColor : IVertexType
    {
        private static readonly VertexDeclaration VertexDeclarationValue = new VertexDeclaration(
            36,
            new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
            new VertexElement(12, VertexElementFormat.Vector3, VertexElementUsage.Normal, 0),
            new VertexElement(24, VertexElementFormat.Vector3, VertexElementUsage.Color, 0)
        );

        public VertexDeclaration VertexDeclaration => VertexDeclarationValue;

        public Vector3 Position { get; set; }
        public Vector3 Normal { get; set; }
        public Vector3 Color { get; set; }

        public VertexPositionNormalColor(Vector3 position, Vector3 normal, Vector3 color)
        {
            Position = position;
            Normal = normal;
            Color = color;
        }

    }
}
