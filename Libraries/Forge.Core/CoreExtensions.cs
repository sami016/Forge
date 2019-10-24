using Forge.Core.Interfaces;
using Forge.Core.Resources;
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

            builder.AddSingleton<ResourceManager<SpriteFont>>(() => new ResourceManager<SpriteFont>());
            builder.AddSingleton<ResourceManager<Color>>(() => new ResourceManager<Color>());

            return builder;
        }
    }
}
