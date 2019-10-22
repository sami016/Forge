using Forge.Core;
using System;

namespace SampleUI
{
    class Program
    {
        static ForgeGame Game => new ForgeGameBuilder()
                .Create();

        [STAThread]
        static void Main(string[] args)
        {
            using (var game = Game)
            {
                game.Run();
            }
        }
    }
}
