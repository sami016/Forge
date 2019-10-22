using System;
using System.Collections.Generic;
using System.Text;
using Forge.UI.Glass.Elements;
using Microsoft.Xna.Framework;

namespace Forge.UI.Glass.Templates
{
    public abstract class Template : ITemplate
    {
        public IElement Current { get; set; }

        public IList<IElement> Children => Current.Children;

        public Template()
        {
        }

        public virtual void Initialise()
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

        public void Render(RenderContext context) => Current?.Render(context);
    }
}
