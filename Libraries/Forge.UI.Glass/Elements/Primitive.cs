using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Forge.Core;
using Forge.Core.Engine;
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
        public Entity Entity { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

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

        public virtual void Tick(TickContext context)
        {
            foreach (var child in Children)
            {
                child.Tick(context);
            }
        }
    }
}
