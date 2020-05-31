using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Forge.UI.Glass.Interaction
{
    public abstract class MouseUIEvent
    {
        public Vector2 MousePosition { get; }
        public bool Propagate { get; private set; } = true;

        public MouseUIEvent(Vector2 mousePosition)
        {
            MousePosition = mousePosition;
        }

        public void PreventPropagate()
        {
            Propagate = false;
        }

        public abstract MouseUIEvent Clone();
    }
}
