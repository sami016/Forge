using Forge.Core.Components;
using Forge.Core.Space.Shapes;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Forge.Core.Space.Bodies
{
    public abstract class Body : Component
    {
        public IShape3 Shape { get; set; }
        public abstract Space Space { get; set; }
        public byte Layer { get; set; }
        public Matrix? Offset { get; protected set; }
    }
}
