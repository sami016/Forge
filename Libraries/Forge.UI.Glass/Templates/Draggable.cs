using Forge.UI.Glass.Elements;
using Forge.UI.Glass.Interaction;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Forge.UI.Glass.Templates
{
    public class Draggable : Template
    {
        private Func<IElement> _notDraggingEval;
        private Func<IElement> _draggingEval;
        private Func<IElement> _cursorLockElementEval;
        private DragAndDropCapatility _dragAndDropCapability;

        private bool _isDragging = false;
        internal bool IsDragging
        {
            get => _isDragging;
            set
            {
                _isDragging = value;
                Reevaluate();
            }
        }

        public Draggable()
        {
        }

        protected override void DoInitialise(UIInitialiseContext uIInitialiseContext)
        {
            _dragAndDropCapability = uIInitialiseContext.GetService<DragAndDropCapatility>();
        }

        public Draggable DefaultAppearance(Func<IElement> notDraggingEval)
        {
            _notDraggingEval = notDraggingEval;
            return this;
        }

        public Draggable DraggingAppearance(Func<IElement> draggingEval)
        {
            _draggingEval = draggingEval;
            return this;
        }

        public Draggable CursorLockElementAppearance(Func<IElement> cursorLockElementEval)
        {
            _cursorLockElementEval = cursorLockElementEval;
            return this;
        }

        public IElement EvalutateCursor()
        {
            return (_cursorLockElementEval ?? _notDraggingEval)?.Invoke();
        }

        public override IElement Evaluate()
        {
            return new Pane(
                    IsDragging
                        ? _draggingEval()
                        : _notDraggingEval()
                )
                .ConfigureEvents(events =>
                {
                    events.Subscribe<MouseDownEvent>(OnMouseDown);
                });
        }

        private void OnMouseDown(MouseDownEvent obj)
        {
            _dragAndDropCapability.TryStartDragging(this);
        }
    }
}
