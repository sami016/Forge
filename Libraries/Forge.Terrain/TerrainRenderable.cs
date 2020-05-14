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
        protected GpuData GpuData;
        private BasicEffect _basicEffect;
        private Matrix _scaleMatrix = Matrix.CreateScale(1f);
        private float _scale = 1f;

        public uint RenderOrder { get; } = RenderableOrdering.GeometryMain;

        public bool AutoRender { get; } = true;

        [Inject] public Transform Transform { get; set; }
        [Inject] public GraphicsDevice GraphicsDevice { get; set; }
        [Inject] public TerrainMeshHelpers TerrainMeshHelpers { get; set; }

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
            if (GraphicsDevice == null)
            {
                throw new Exception($"{nameof(GraphicsDevice)} was not injected");
            }
            _basicEffect = new BasicEffect(GraphicsDevice);
            _basicEffect.EnableDefaultLighting();
        }

        public virtual void SetGpuData(GpuData gpuData)
        {
            GpuData = gpuData;
        }

        public virtual void SetHeightData(Bitmap[,] surroundingHeightmaps)
        {
            GpuData = TerrainMeshHelpers.CreateTerrainMesh(surroundingHeightmaps);
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
                GpuData.RenderTriangleList(GraphicsDevice);
            }
        }
    }
}
