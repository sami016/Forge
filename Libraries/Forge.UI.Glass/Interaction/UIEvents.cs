using System;
using System.Collections.Generic;
using System.Text;

namespace Forge.UI.Glass.Interaction
{
    /// <summary>
    /// Responsible for maintaining event subscriptions within a component.
    /// </summary>
    public class UIEvents
    {
        private readonly Dictionary<Type, IList<object>> _handlers = new Dictionary<Type, IList<object>>();

        public UIEvents Subscribe<TEvent>(Action<TEvent> handler)
        {
            if (!_handlers.ContainsKey(typeof(TEvent)))
            {
                _handlers[typeof(TEvent)] = new List<object>();
            }
            _handlers[typeof(TEvent)].Add(handler);
            return this;
        }

        public void Unsubscribe<TEvent>(Action<TEvent> handler)
        {
            if (!_handlers.ContainsKey(typeof(TEvent)))
            {
                return;
            }
            _handlers[typeof(TEvent)].Remove(handler);
        }

        public bool Handles<TEvent>()
        {
            return _handlers.ContainsKey(typeof(TEvent));
        }

        public void Handle<TEvent>(TEvent eventObject)
        {
            if (_handlers.ContainsKey(typeof(TEvent)))
            {
                foreach (var val in _handlers[typeof(TEvent)])
                {
                    var action = val as Action<TEvent>;
                    action?.Invoke(eventObject);
                }
            }
        }
    }
}
