using Forge.Core.Components;
using Forge.Core.Rendering.VertexTypes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Forge.Core.Rendering
{
    public class RenderResources : Component
    {
        [Inject] public GraphicsDevice GraphicsDevice { get; set; }

        private Texture2D _whiteTexture;
        public Texture2D WhiteTexture
        {
            get
            {
                if (_whiteTexture != null)
                {
                    return _whiteTexture;
                }
                var texture = new Texture2D(GraphicsDevice, 1, 1);
                var data = new Color[1];
                data[0] = Color.White;
                texture.SetData<Color>(data);
                _whiteTexture = texture;
                return texture;
            }
        }

        private Texture2D _blackTexture;
        public Texture2D BlackTexture
        {
            get
            {
                if (_blackTexture != null)
                {
                    return _blackTexture;
                }
                var texture = new Texture2D(GraphicsDevice, 1, 1);
                var data = new Color[1];
                data[0] = Color.Black;
                texture.SetData<Color>(data);
                _blackTexture = texture;
                return texture;
            }
        }

        private GpuData? _square_textureOnly;
        public GpuData Square_TextureOnly()
        {
            return _square_textureOnly
                   ?? (_square_textureOnly = GpuData.Create(
                       new IndexBufferBuilder<ushort>(6)
                           .AddNext(0, 1, 2)
                           .AddNext(1, 3, 2),
                       new VertexBufferBuilder<VertexTexture>(4)
                           .AddNext(new VertexTexture(new Vector2(0, 1)))
                           .AddNext(new VertexTexture(new Vector2(1, 1)))
                           .AddNext(new VertexTexture(new Vector2(0, 0)))
                           .AddNext(new VertexTexture(new Vector2(1, 0))),
                       GraphicsDevice
                   )).Value;
        }


        private GpuData? _cube_PositionNormalTexture;
        public GpuData Cube_PositionNormalTexture()
        {
            return _cube_PositionNormalTexture
                   ?? (_cube_PositionNormalTexture = GpuData.Create(
                       new IndexBufferBuilder<ushort>(6 * 6)
                           .AddNext(0, 2, 1)
                           .AddNext(0, 3, 2)
                           .AddNext(4 + 0, 4 + 1, 4 + 2)
                           .AddNext(4 + 0, 4 + 2, 4 + 3)
                           .AddNext(8 + 0, 8 + 2, 8 + 1)
                           .AddNext(8 + 0, 8 + 3, 8 + 2)
                           .AddNext(12 + 0, 12 + 1, 12 + 2)
                           .AddNext(12 + 0, 12 + 2, 12 + 3)
                           .AddNext(16 + 0, 16 + 2, 16 + 1)
                           .AddNext(16 + 0, 16 + 3, 16 + 2)
                           .AddNext(20 + 0, 20 + 1, 20 + 2)
                           .AddNext(20 + 0, 20 + 2, 20 + 3)
                           ,
                       new VertexBufferBuilder<VertexPositionNormalTexture>(24)
                        // Top
                        .AddNext(new VertexPositionNormalTexture(new Vector3(1, 1, 1), Vector3.Up, new Vector2(0, 1)))
                        .AddNext(new VertexPositionNormalTexture(new Vector3(1, 1, -1), Vector3.Up, new Vector2(1, 1)))
                        .AddNext(new VertexPositionNormalTexture(new Vector3(-1, 1, -1), Vector3.Up, new Vector2(0, 0)))
                        .AddNext(new VertexPositionNormalTexture(new Vector3(-1, 1, 1), Vector3.Up, new Vector2(1, 0)))
                        // Bottom
                        .AddNext(new VertexPositionNormalTexture(new Vector3(1, -1, 1), Vector3.Down, new Vector2(0, 1)))
                        .AddNext(new VertexPositionNormalTexture(new Vector3(1, -1, -1), Vector3.Down, new Vector2(1, 1)))
                        .AddNext(new VertexPositionNormalTexture(new Vector3(-1, -1, -1), Vector3.Down, new Vector2(0, 0)))
                        .AddNext(new VertexPositionNormalTexture(new Vector3(-1, -1, 1), Vector3.Down, new Vector2(1, 0)))
                        // Left
                        .AddNext(new VertexPositionNormalTexture(new Vector3(-1, 1, 1), Vector3.Left, new Vector2(0, 1)))
                        .AddNext(new VertexPositionNormalTexture(new Vector3(-1, 1, -1), Vector3.Left, new Vector2(1, 1)))
                        .AddNext(new VertexPositionNormalTexture(new Vector3(-1, -1, -1), Vector3.Left, new Vector2(0, 0)))
                        .AddNext(new VertexPositionNormalTexture(new Vector3(-1, -1, 1), Vector3.Left, new Vector2(1, 0)))
                        // Right
                        .AddNext(new VertexPositionNormalTexture(new Vector3(1, 1, 1), Vector3.Right, new Vector2(0, 1)))
                        .AddNext(new VertexPositionNormalTexture(new Vector3(1, 1, -1), Vector3.Right, new Vector2(1, 1)))
                        .AddNext(new VertexPositionNormalTexture(new Vector3(1, -1, -1), Vector3.Right, new Vector2(0, 0)))
                        .AddNext(new VertexPositionNormalTexture(new Vector3(1, -1, 1), Vector3.Right, new Vector2(1, 0)))
                        // Forward
                        .AddNext(new VertexPositionNormalTexture(new Vector3(1, 1, -1), Vector3.Forward, new Vector2(0, 1)))
                        .AddNext(new VertexPositionNormalTexture(new Vector3(1, -1, -1), Vector3.Forward, new Vector2(1, 1)))
                        .AddNext(new VertexPositionNormalTexture(new Vector3(-1, -1, -1), Vector3.Forward, new Vector2(0, 0)))
                        .AddNext(new VertexPositionNormalTexture(new Vector3(-1, 1, -1), Vector3.Forward, new Vector2(1, 0)))
                        // Backward
                        .AddNext(new VertexPositionNormalTexture(new Vector3(1, 1, 1), Vector3.Backward, new Vector2(0, 1)))
                        .AddNext(new VertexPositionNormalTexture(new Vector3(1, -1, 1), Vector3.Backward, new Vector2(1, 1)))
                        .AddNext(new VertexPositionNormalTexture(new Vector3(-1, -1, 1), Vector3.Backward, new Vector2(0, 0)))
                        .AddNext(new VertexPositionNormalTexture(new Vector3(-1, 1, 1), Vector3.Backward, new Vector2(1, 0))),
                       GraphicsDevice
                   )).Value;
        }

    }
}
