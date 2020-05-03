using Forge.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Forge.Terrain
{
    public static class TerrainExtensions
    {
        public static ForgeGameBuilder UseTerrain(this ForgeGameBuilder gameBuilder)
        {
            gameBuilder.AddSingleton(() => new TerrainMeshHelpers());

            return gameBuilder;
        }
    }
}
