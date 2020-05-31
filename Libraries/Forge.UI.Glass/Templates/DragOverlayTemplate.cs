using Forge.Core;
using Forge.Core.Interfaces;
using Forge.UI.Glass.Elements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Forge.UI.Glass.Templates
{
    public class DragOverlayTemplate : Template
    {
        private readonly DragAndDropCapatility _dragAndDropCapatility;

        public DragOverlayTemplate(DragAndDropCapatility dragAndDropCapatility)
        {
            _dragAndDropCapatility = dragAndDropCapatility;

            _dragAndDropCapatility.CurrentlyDraggingChanged += _dragAndDropCapatility_CurrentDraggingChanged;
        }

        private void _dragAndDropCapatility_CurrentDraggingChanged()
        {
            Reevaluate();
        }

        public override IElement Evaluate()
        {
            var mouseState = Mouse.GetState();
            var el = _dragAndDropCapatility.CurrentlyDragging;
            if (el == null)
            {
                return new Pane();
            }
            var cursorEl = el.EvalutateCursor();
            if (cursorEl == null)
            {
                return new Pane();
            }
            Position = new Rectangle(mouseState.X, mouseState.Y, cursorEl.Position.Width, cursorEl.Position.Height);
            return cursorEl;
        }

        public override void Tick(TickContext context)
        {
            base.Tick(context);

            if (_dragAndDropCapatility.CurrentlyDragging != null)
            {
                Reevaluate();
            }
        }

        public override void Dispose()
        {
            _dragAndDropCapatility.CurrentlyDraggingChanged -= _dragAndDropCapatility_CurrentDraggingChanged;

            base.Dispose();
        }
    }
}
