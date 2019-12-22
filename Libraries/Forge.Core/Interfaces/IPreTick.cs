using Forge.Core.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace Forge.Core.Interfaces
{
    public interface IPreTick
    {
        void PreTick(TickContext context);
    }
}
