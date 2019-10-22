using Forge.Core.Resources;
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
            builder.AddSingleton<ResourceManager<SpriteFont>>(() => new ResourceManager<SpriteFont>());

            return builder;
        }
    }
}
