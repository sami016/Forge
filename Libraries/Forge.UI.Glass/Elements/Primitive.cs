using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Forge.Core;
using Forge.Core.Engine;
using Forge.UI.Glass.Interaction;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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
            Children = (children ?? new IElement[0]).ToList();
        }

        public virtual void Initialise(UIInitialiseContext uiInitialiseContext)
        {
            foreach (var element in Children) {
                element.Initialise(uiInitialiseContext);
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
