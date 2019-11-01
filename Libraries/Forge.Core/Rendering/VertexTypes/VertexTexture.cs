using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Forge.Core.Rendering.VertexTypes
{
    // Corresponds with VS_TextureOnly.
    public struct VertexTexture : IVertexType
    {
        private static readonly VertexDeclaration VertexDeclarationValue = new VertexDeclaration(
            8,
            new VertexElement(0, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0)
        );

        public VertexDeclaration VertexDeclaration => VertexDeclarationValue;

        public Vector2 UV { get; set; }

        public VertexTexture(Vector2 uv)
        {
            UV = uv;
        }

    }
}
