using Forge.Core.Engine;
using System;
using System.Collections.Generic;
using System.Text;

namespace Forge.Core.Components
{
    public interface IComponent : IDisposable
    {
        Entity Entity { get; set; }
    }
}
