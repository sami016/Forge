using System;
using System.Collections.Generic;
using System.Text;
using Forge.UI.Glass.Elements;
using Forge.UI.Glass.Interaction;
using Microsoft.Xna.Framework;

namespace Forge.UI.Glass.Templates
{
    public abstract class Template : ITemplate
    {
        public IElement Current { get; set; }
        public Rectangle Position { 
            get => Current.Position; 
            set
            {
                Current.Position = value;
            }
        }

        public IList<IElement> Children => Current.Children;

        public UIEvents Events => Current.Events;

        public Action<IElement> Init { 
            get => Current.Init;
            set
            {
                Current.Init = value;
            }
        }

        public Template()
        {
        }

        public virtual void PreEvaluate()
        {
        }

        public void Reevaluate()
        {
            try
            {
                Current = Evaluate();
            }catch(Exception ex)
            {
                throw new Exception($"An exception occured whilst evaluating a UI template", ex);
            }
        }

        public abstract IElement Evaluate();

        public virtual void Dispose()
        {
            foreach (var child in Children)
            {
                if (child is IDisposable disposable)
                {
                    disposable.Dispose();
                }
            }
            Current = null;
        }

        public void Render(UIRenderContext context) => Current?.Render(context);

        public void Initialise() => Current?.Initialise();
    }
}
