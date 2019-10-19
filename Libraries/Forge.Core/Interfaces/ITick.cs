using System;
using System.Collections.Generic;
using System.Text;

namespace Forge.Core.Interfaces
{
    public interface ITick
    {
        void Tick(TickContext context);
    }
}
