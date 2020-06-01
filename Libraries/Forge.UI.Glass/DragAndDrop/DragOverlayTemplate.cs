using Forge.Core;
using Forge.Core.Interfaces;
using Forge.UI.Glass.Elements;
using Forge.UI.Glass.Templates;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Forge.UI.Glass.DragAndDrop
{
    public class DragOverlayTemplate : Template
    {
        private readonly DragAndDropCapatility _dragAndDropCapatility;
        private Vector2? _currentPos;

        public DragOverlayTemplate(DragAndDropCapatility dragAndDropCapatility)
        {
            _dragAndDropCapatility = dragAndDropCapatility;

            _dragAndDropCapatility.CurrentlyDraggingChanged += _dragAndDropCapatility_CurrentDraggingChanged;
        }

        private void _dragAndDropCapatility_CurrentDraggingChanged()
        {
            _currentPos = null;
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

            if (!_currentPos.HasValue)
            {
                _currentPos = new Vector2(mouseState.X, mouseState.Y);
            }

            switch (el.DragOptions.DragTrailType)
            {
                case DragTrailType.None:
                    _currentPos = new Vector2(mouseState.X, mouseState.Y);
                    break;
                case DragTrailType.Linear:
                    {
                        var moveRate = el.DragOptions.DragTrailRate * TickContext.DeltaTimeSeconds;
                        var thresholdSq = moveRate * moveRate;

                        var targetPos = new Vector2(mouseState.X, mouseState.Y);
                        var moveDir = (targetPos - _currentPos.Value);
                        if (moveDir.LengthSquared() > thresholdSq)
                        {
                            moveDir.Normalize();
                            _currentPos = _currentPos.Value + moveDir * moveRate;
                            // Snap to target once close.
                            if ((_currentPos.Value - targetPos).LengthSquared() < thresholdSq)
                            {
                                _currentPos = targetPos;
                            }
                            Console.WriteLine(_currentPos.Value);
                        }
                    }
                    break;
                case DragTrailType.Proportional:
                    {

                        var targetPos = new Vector2(mouseState.X, mouseState.Y);
                        var moveDir = (targetPos - _currentPos.Value);
                        var moveDist = moveDir.Length();
                        var moveRate = el.DragOptions.DragTrailRate * TickContext.DeltaTimeSeconds * moveDist;
                        var thresholdSq = moveRate * moveRate;
                        if (moveDir.LengthSquared() > thresholdSq)
                        {
                            moveDir.Normalize();
                            _currentPos = _currentPos.Value + moveDir * moveRate;
                            // Snap to target once close.
                            if ((_currentPos.Value - targetPos).LengthSquared() < thresholdSq)
                            {
                                _currentPos = targetPos;
                            }
                            Console.WriteLine(_currentPos.Value);
                        }

                    }
                    break;
            }


            Position = new Rectangle((int)_currentPos.Value.X, (int)_currentPos.Value.Y, cursorEl.Position.Width, cursorEl.Position.Height);
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
