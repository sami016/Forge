using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Forge.Core.Utilities
{
    public static class ModelExtensions
    {
        public static void EnableDefaultLighting(this Model model)
        {
            foreach (var mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                }
            }
        }

        public static void SetDiffuseColour(this Model model, Color color)
        {
            foreach (var mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.DiffuseColor = color.ToVector3();
                }
            }
        }
        public static void ConfigureBasicEffects(this Model model, Action<BasicEffect> configureEffect)
        {
            foreach (var mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    configureEffect?.Invoke(effect);
                }
            }
        }

    }
}
