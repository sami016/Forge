using System;
using System.Collections.Generic;
using System.Text;

namespace Forge.Core
{
    /// <summary>
    /// 
    /// </summary>
    public class EngineHooks
    {
        public event Action Render;
        public event Action Tick;

        public EngineHooks()
        {

        }
    }
}
