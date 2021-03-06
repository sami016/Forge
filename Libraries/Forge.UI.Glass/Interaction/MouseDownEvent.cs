﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Forge.UI.Glass.Interaction
{
    public class MouseDownEvent : MouseUIEvent
    {
        public MouseDownEvent(Vector2 mousePosition) : base(mousePosition)
        {
        }

        public override MouseUIEvent Clone()
        {
            return new MouseDownEvent(MousePosition);
        }
    }
}
