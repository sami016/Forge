using Forge.Core.Components;
using Forge.Core.Engine;
using Forge.Core.Interfaces;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Forge.Core.Utilities { 

    public class KeyControls : Component, IPostTick
    {
        private IDictionary<Keys, bool> _lastState = new Dictionary<Keys, bool>();
        private IDictionary<Keys, bool> _currentState = new Dictionary<Keys, bool>();

        public void PostTick(TickContext context)
        {
            var keyboard = Keyboard.GetState();
            _lastState = _currentState;
            _currentState = new Dictionary<Keys, bool>();
            foreach (Keys key in Enum.GetValues(typeof(Keys)))
            {
                _lastState[key] = keyboard.IsKeyDown(key);
            }
        }

        public bool HasBeenPressed(Keys key)
        {
            var keyboard = Keyboard.GetState();
            return keyboard.IsKeyDown(key) && !_lastState[key];
        }

        public bool HasBeenReleased(Keys key)
        {
            var keyboard = Keyboard.GetState();
            return keyboard.IsKeyUp(key) && _lastState[key];
        }
    }
}
