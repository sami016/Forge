using System;
using System.Drawing;
using Forge.Core.Components;
using Forge.Core.Engine;
using Forge.Core.Interfaces;
using Forge.Core.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Forge.Terrain
{
    public class TerrainRenderable : Component, IInit, IRenderable
    {
        private GpuData _gpuData;
        private BasicEffect _basicEffect;
        private Matrix _scaleMatrix = Matrix.CreateScale(1f);
        private float _scale = 1f;

        public uint RenderOrder { get; } = RenderableOrdering.GeometryMain;

        public bool AutoRender { get; } = true;

        [Inject] Transform Transform { get; set; }
        [Inject] GraphicsDevice GraphicsDevice { get; set; }
        [Inject] TerrainMeshHelpers TerrainMeshHelpers { get; set; }

        public float Scale
        {
            get => _scale;
            set {
                _scale = value;
                _scaleMatrix = Matrix.CreateScale(value);
            }
        }

        public void Initialise()
        {
            _basicEffect = new BasicEffect(GraphicsDevice);
            _basicEffect.EnableDefaultLighting();
            var surroundings = new Bitmap[3, 3];
            for (var i=0; i<3; i++)
            {
                for (var j=0; j<3; j++)
                {
                    surroundings[i, j] = new System.Drawing.Bitmap(5, 5);
                }
            }
            _gpuData = TerrainMeshHelpers.CreateTerrainMesh(surroundings); 
        }

        public void Render(RenderContext context)
        {
            if (context.View == null || context.Projection == null)
            {
                return;
            }

            _basicEffect.World = _scaleMatrix * Transform.WorldTransform;
            _basicEffect.View = context.View.Value;
            _basicEffect.Projection = context.Projection.Value;
            foreach (var pass in _basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                _gpuData.RenderTriangleList(GraphicsDevice);
            }
        }
    }
}
