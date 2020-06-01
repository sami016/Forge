using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Forge.UI.Glass.DragAndDrop
{
    public class DragOptions
    {
        public Vector2 CursorPixelOffset { get; set; } = Vector2.Zero;
        public Vector2 CursorPercentageOffset { get; set; } = Vector2.Zero;

        public DragTrailType DragTrailType { get; set; }
        public float DragTrailRate { get; set; } = 0f;
    }
}
