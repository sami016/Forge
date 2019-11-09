using Forge.Core.Engine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Forge.Core.Space
{
    public interface ISpace
    {
        IEnumerable<Entity> GetNearby(Vector3 location, float radius);
    }
}
