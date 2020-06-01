using Forge.Core;
using Forge.Core.Components;
using Forge.Core.Interfaces;
using Forge.UI.Glass.Elements;
using Forge.UI.Glass.Templates;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Forge.UI.Glass.DragAndDrop
{
    public class DragAndDropCapatility : Component, IInit, ITick
    {
        [Inject] UserInterfaceManager UserInterfaceManager { get; set; }
        private MouseState _previousState;


        public event Action CurrentlyDraggingChanged;

        /// <summary>
        /// The element that is actively being dragged.
        /// </summary>
        public Draggable CurrentlyDragging { get; private set; } = null;

        public void Initialise()
        {
            UserInterfaceManager.Create(new DragOverlayTemplate(this), 200);
        }

        public void Tick(TickContext context)
        {
            var state = Mouse.GetState();
            if (_previousState == null)
            {
                _previousState = state;
            }

            // Detect mouse up whilst dragging.
            if (CurrentlyDragging != null)
            {
                if (state.LeftButton == ButtonState.Released
                    && _previousState.LeftButton == ButtonState.Pressed)
                {
                    StopDragging();
                }
            }

            _previousState = state;
        }

        internal void TryStartDragging(Draggable draggable)
        {
            if (CurrentlyDragging != null)
            {
                return;
            }
            CurrentlyDragging = draggable;
            draggable.IsDragging = true;
            CurrentlyDraggingChanged?.Invoke();
        }

        public void StopDragging()
        {
            if (CurrentlyDragging == null)
            {
                return;
            }

            CurrentlyDragging.IsDragging = false;
            CurrentlyDragging = null;
            CurrentlyDraggingChanged?.Invoke();
        }
    }
}
