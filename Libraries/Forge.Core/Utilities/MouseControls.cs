using Forge.Core.Components;
using Forge.Core.Engine;
using Forge.Core.Interfaces;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Forge.Core.Utilities { 

    public class MouseControls : Component, ITick
    {
        private MouseState? _lastState = null;
        private MouseState? _currentState = Mouse.GetState();

        public MouseState MouseState => _currentState.Value;

        public void Tick(TickContext context)
        {
            var mouse = Mouse.GetState();
            _lastState = _currentState;
            _currentState = mouse;
        }

        public bool LeftClicked => _currentState.Value.LeftButton == ButtonState.Pressed
            && (_lastState == null || _lastState.Value.LeftButton == ButtonState.Released);
        public bool RightClicked => _currentState.Value.RightButton == ButtonState.Pressed
            && (_lastState == null || _lastState.Value.RightButton == ButtonState.Released);

        public bool LeftReleased => _currentState.Value.LeftButton == ButtonState.Released
            && (_lastState == null || _lastState.Value.LeftButton == ButtonState.Pressed);
        public bool RightReleased => _currentState.Value.RightButton == ButtonState.Released
            && (_lastState == null || _lastState.Value.RightButton == ButtonState.Pressed);



        public int ScrollWheelValueDelta => _currentState.Value.ScrollWheelValue - _lastState.Value.ScrollWheelValue;
    }
}
