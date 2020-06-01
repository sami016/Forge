using Forge.Core;
using Forge.Core.Components;
using Forge.Core.Interfaces;
using Forge.UI.Glass.Elements;
using Forge.UI.Glass.Interaction;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Forge.UI.Glass
{
    /// <summary>
    /// Singleton to enable mouse events.
    /// </summary>
    public class MouseCapability : Component, ITick
    {
        [Inject] public UserInterfaceManager UserInterfaceManager { get; set; }
        [Inject] public GraphicsDevice GraphicsDevice { get; set; }
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
                && _previousState.LeftButton == ButtonState.Released)
            {
                foreach (var layer in UserInterfaceManager.TemplateLayers.ToArray())
                {
                    // new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height)
                    HitCheckRecurse(layer, layer.Position, position.ToVector2(), (pos) => new MouseDownEvent(pos));
                }
            }
            else if (state.LeftButton == ButtonState.Released
                && _previousState.LeftButton == ButtonState.Pressed)
            {
                foreach (var layer in UserInterfaceManager.TemplateLayers.ToArray())
                {
                    HitCheckRecurse(layer, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), position.ToVector2(), (pos) => new MouseUpEvent(pos));
                }
            }

            _previousState = state;
        }

        private bool HitCheckRecurse<T>(IElement element, Rectangle bounds, Vector2 mousePosition, Func<Vector2, T> eventFunc)
            where T : MouseUIEvent
        {
            var globalPosition = new Rectangle(element.Position.Location + bounds.Location, element.Position.Size);
            // First check, is the click within the element?
            if (HitCheck(globalPosition, mousePosition))
            {
                // Children first.
                foreach (var child in element.Children)
                {
                    if (HitCheckRecurse<T>(child, globalPosition, mousePosition, eventFunc))
                    {
                        return true;
                    }
                }

                // Handle any listener events.
                var @event = eventFunc(mousePosition);
                if (element.Events.Handles<T>())
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
