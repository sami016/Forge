using System;
using System.Collections.Generic;
using System.Text;
using Forge.Core.Engine;

namespace Forge.Core.Components
{
    public class Component : IComponent
    {
        public Entity Entity { get; set; }

        public virtual void Dispose()
        {
        }
    }
}
