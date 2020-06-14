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
        private readonly Dictionary<Type, object> _handlers = new Dictionary<Type, object>();

        public UIEvents Subscribe<TEvent>(Action<TEvent> handler)
        {
            _handlers[typeof(TEvent)] = handler;
            return this;
        }

        public void Unsubscribe<TEvent>()
        {
            _handlers.Remove(typeof(TEvent));
        }

        public bool Handles<TEvent>()
        {
            return _handlers.ContainsKey(typeof(TEvent));
        }

        public void Handle<TEvent>(TEvent eventObject)
        {
            var action = _handlers[typeof(TEvent)] as Action<TEvent>;
            action?.Invoke(eventObject);
        }

        public void Handles<T>(Func<object, object> p)
        {
            throw new NotImplementedException();
        }
    }
}
