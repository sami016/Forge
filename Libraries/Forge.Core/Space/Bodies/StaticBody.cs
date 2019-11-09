using Forge.Core.Components;
using Forge.Core.Space.Shapes;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Forge.Core.Space.Bodies
{
    public class StaticBody : Body
    {
        [Inject] public override Space Space { get; set; }

        public StaticBody()
        {

        }

        public StaticBody(IShape3 shape, byte layer = 0, Matrix? offset = null)
        {
            Shape = shape;
            Layer = layer;
            Offset = offset;
        }
    }
}
