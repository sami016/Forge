﻿using Forge.Core;
using Forge.UI.Glass;
using System;

namespace SampleGame
{
    public static class Program
    {
        static ForgeGame Game => new ForgeGameBuilder()
                .UseEnginePrimitives()
                .UseGlassUI()
                .WithInitialScene(() => new MenuScene())
                .Create();

        [STAThread]
        static void Main()
        {
            using (var game = Game)
                game.Run();
        }
    }
}
