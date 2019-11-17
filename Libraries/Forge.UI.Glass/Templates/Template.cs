using System;
using System.Collections.Generic;
using System.Text;
using Forge.Core;
using Forge.UI.Glass.Elements;
using Forge.UI.Glass.Interaction;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Forge.UI.Glass.Templates
{
    public abstract class Template : ITemplate
    {
        public IElement Current { get; set; }
        public GraphicsDevice GraphicsDevice { get; private set; }

        private Rectangle? _position;
        private Action<IElement> _init;

        public Rectangle Position { 
            get => Current.Position;
            set
            {
                if (Current != null)
                {
                    Current.Position = value;
                } else
                {
                    _position = value;
                }
            }
        }

        public Action<IElement> Init
        {
            get => Current.Init;
            set
            {
                if (Current != null)
                {
                    Current.Init = value;
                }
                else
                {
                    _init = value;
                }
            }
        }

        public IList<IElement> Children => Current.Children;

        public UIEvents Events => Current.Events;

        public Template()
        {
        }

        public void Initialise(GraphicsDevice graphicsDevice)
        {
            GraphicsDevice = graphicsDevice;
            PreEvaluate();
            Reevaluate();
        }

        public virtual void PreEvaluate()
        {
        }

        public void Reevaluate()
        {
            try
            {
                Current = Evaluate();
                if (_position.HasValue)
                {
                    Current.Position = _position.Value;
                }
                if (_init != null)
                {
                    Current.Init = _init;
                }
                Current.Initialise(GraphicsDevice);
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

        public virtual void Tick(TickContext context)
        {
            Current?.Tick(context);
        }

        public void Render(UIRenderContext context) => Current?.Render(context);
    }
}
