using Forge.Core;
using Forge.Core.Scenes;
using Forge.UI.Glass.Elements;
using Forge.UI.Glass.Templates;
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
            gameBuilder.AddSingleton(() => new MouseCapability());
            gameBuilder.AddSingleton(() => new DragAndDropCapatility());

            return gameBuilder;
        }

        public static void AddSceneUI(this UserInterfaceManager userInterfaceManager, Scene scene, Template template)
        {
            var uiDispose = userInterfaceManager.Create(template);
            scene.AddDisposeAction(() => uiDispose());
        }
    }
}
