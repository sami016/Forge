using System;
using System.Collections.Generic;
using System.Text;

namespace Forge.Core.Engineware
{
    public class TickableEngineware : IEngineware
    {

        public void Initialise(EngineHooks engine)
        {
            engine.Tick += OnTick;
        }

        private void OnTick()
        {
        }

        public void Dispose()
        {
        }
    }
}
