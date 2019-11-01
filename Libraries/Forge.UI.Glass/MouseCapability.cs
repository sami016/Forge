using Forge.Core;
using Forge.Core.Components;
using Forge.Core.Interfaces;
using Forge.UI.Glass.Elements;
using Forge.UI.Glass.Interaction;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Forge.UI.Glass
{
    /// <summary>
    /// Singleton to enable mouse events.
    /// </summary>
    public class MouseCapability : Component, ITick
    {
        [Inject] public UserInterfaceManager UserInterfaceManager { get; set; }
        private MouseState _previousState;

        public MouseCapability()
        {

        }

        public void Tick(TickContext context)
        {
            var state = Mouse.GetState();
            if (_previousState == null)
            {
                _previousState = state;
            }
            var position = state.Position;

            if (state.LeftButton == ButtonState.Pressed 
                && _previousState.LeftButton == ButtonState.Released) {
                foreach (var layer in UserInterfaceManager.TemplateLayers)
                {
                    HitCheckRecurse(layer, Vector2.Zero, position.ToVector2());
                }
            }

            _previousState = state;
        }

        private bool HitCheckRecurse(IElement element, Vector2 offset, Vector2 mousePosition)
        {
            // First check, is the click within the element?
            if (HitCheck(element.Position, mousePosition))
            {
                // Children first.
                foreach (var child in element.Children)
                {
                    if (HitCheckRecurse(child, offset + element.Position.Location.ToVector2(), mousePosition))
                    {
                        return true;
                    }
                }

                // Handle any listener events.
                var @event = new ClickUIEvent(mousePosition);
                if (element.Events.Handles<ClickUIEvent>())
                {
                    element.Events.Handle(@event);
                    return !@event.Propagate;
                }
            }
            return false;
        }

        private static bool HitCheck(Rectangle rectangle, Vector2 point)
        {
            return !(point.X < rectangle.Left
                || point.X > rectangle.Right
                || point.Y < rectangle.Top
                || point.Y > rectangle.Bottom);
        }
    }
}
