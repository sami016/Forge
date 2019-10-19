using System;
using System.Collections.Generic;
using System.Text;

namespace Forge.Core.Engineware
{
    /// <summary>
    /// A module that interfaces between the engine and the game logic.
    /// </summary>
    public interface IEngineware : IDisposable
    {
        void Initialise(EngineHooks engine);
        void Dispose();
    }
}
