using Forge.Core.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace Forge.Core.Interfaces
{
    public interface IPostTick
    {
        void PostTick(TickContext context);
    }
}
