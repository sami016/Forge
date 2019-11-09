using Forge.Core.Interfaces;
using Forge.Core.Rendering;
using Forge.Core.Rendering.Cameras;
using Forge.Core.Resources;
using Forge.Core.Space;
using Forge.Core.Space.Bodies;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Forge.Core
{
    public static class CoreExtensions
    {
        public static ForgeGameBuilder UseEnginePrimitives(this ForgeGameBuilder builder)
        {
            // Index all components that tick for speed.
            builder.IndexInterface<ITick>();
            builder.IndexInterface<IRenderable>();

            builder.AddSingleton<ResourceManager<SpriteFont>>(() => new ResourceManager<SpriteFont>());
            builder.AddSingleton<ResourceManager<Color>>(() => new ResourceManager<Color>());
            builder.AddSingleton<ResourceManager<Texture2D>>(() => new ResourceManager<Texture2D>());
            builder.AddSingleton<RenderResources>(() => new RenderResources());
            builder.AddSingleton<CameraManager>(() => new CameraManager());


            return builder;
        }

        public static ForgeGameBuilder UseCollisionPrimitives(this ForgeGameBuilder builder)
        {
            // Index all components that tick for speed.
            builder.IndexInterface<Body>();

            builder.AddSingleton<RayCollider>(() => new RayCollider());
            builder.AddSingleton<ISpace>(() => new Space.Space());

            return builder;
        }
    }
}
