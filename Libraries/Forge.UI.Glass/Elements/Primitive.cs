using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Forge.UI.Glass.Interaction;
using Microsoft.Xna.Framework;

namespace Forge.UI.Glass.Elements
{
    /// <summary>
    /// Element primitive abstract implementation.
    /// </summary>
    public abstract class Primitive : IElement
    {
        public IList<IElement> Children { get; }
        public Rectangle Position { get; set; }
        public UIEvents Events { get; } = new UIEvents();
        public Action<IElement> Init { get; set; }

        public Primitive(IEnumerable<IElement> children)
        {
            Children = children.ToList();
        }

        public void Initialise()
        {
            foreach (var element in Children) {
                element.Initialise();
            } 
            Init?.Invoke(this);
        }

        public abstract void Render(UIRenderContext context);
    }
}
