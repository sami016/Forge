using Forge.Core;
using Forge.UI.Glass.Elements;
using System;
using System.Collections.Generic;
using System.Text;

namespace Forge.UI.Glass
{
    public static class GlassExtensions
    {
        public static ForgeGameBuilder UseGlassUI(this ForgeGameBuilder gameBuilder)
        {
            gameBuilder.IndexInterface<IElement>();
            gameBuilder.AddSingleton(() => new UserInterfaceManager());

            return gameBuilder;
        }
    }
}
