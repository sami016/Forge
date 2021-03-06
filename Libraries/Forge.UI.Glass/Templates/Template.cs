﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Forge.Core;
using Forge.UI.Glass.Elements;
using Forge.UI.Glass.Interaction;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Forge.UI.Glass.Templates
{
    /// <summary>
    /// A template is a type of element that creates a tree of elements each time it is evaluated.
    /// Templates can be reevaluated in response to a change in the game state - how this is done is left to the user.
    /// For continual updates, the template may be reevaluated every tick, although this is less efficient compared to reevaluating only in response to relevant change.
    /// 
    /// All templates are elements, and will render by calling the render method of the root of the evaluated tree.
    /// </summary>
    public abstract class Template : ITemplate
    {
        public IElement Current { get; set; }

        private bool _initialised;
        private UIInitialiseContext _uiInitialiseContext;

        public GraphicsDevice GraphicsDevice { get; private set; }

        private Rectangle? _position;
        private Action<IElement> _init;

        public Rectangle Position { 
            get => Current?.Position ?? _position ?? new Rectangle(0, 0, 0, 0);
            set
            {
                _position = value;
                if (Current != null)
                {
                    Current.Position = value;
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

        public IList<IElement> Children => Current?.Children ?? new IElement[0];

        public UIEvents Events { get; set; } = new UIEvents();

        public float Vw => GraphicsDevice.Viewport.Width / 100f;
        public float Vh => GraphicsDevice.Viewport.Height / 100f;
        public float Pw => Position.Width / 100f;
        public float Ph => Position.Height / 100f;

        protected TickContext TickContext { get; private set; }

        public Template()
        {
        }

        /// <summary>
        /// Initialises the template.
        /// </summary>
        /// <param name="graphicsDevice"></param>
        public virtual void Initialise(UIInitialiseContext uiInitialiseContext)
        {
            _initialised = true;
            _uiInitialiseContext = uiInitialiseContext;
            DoInitialise(_uiInitialiseContext);
            GraphicsDevice = uiInitialiseContext.GraphicsDevice;
            PreEvaluate();
            Reevaluate();
        }

        /// <summary>
        /// Perform any required service injection here.
        /// </summary>
        /// <param name="uIInitialiseContext">ui initialise context</param>
        protected virtual void DoInitialise(UIInitialiseContext uIInitialiseContext)
        {
        }

        public virtual void PreEvaluate()
        {
        }

        public void Reevaluate()
        {
            if (!_initialised)
            {
                return;
            }
            try
            {
                try
                {
                    Current = Evaluate();
                } 
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error evaluating ui tmeplate: {ex.Message}\n{ex.StackTrace}");
                }
                if (Current != null)
                {
                    Current.Events = Events;
                }
                Current?.Initialise(_uiInitialiseContext);
                if (_position.HasValue)
                {
                    Current.Position = _position.Value;
                } 
                if (Current.Children.Any())
                {
                    if (Current.Position.Width == 0
                        || Current.Position.Height == 0)
                    {
                        Current.SizeToChildren();
                    }
                }
                if (_init != null)
                {
                    Current.Init = _init;
                }

                Current.Initialise(_uiInitialiseContext);
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
            TickContext = context;
        }

        public void Render(UIRenderContext context) => Current?.Render(context);
        public void Prerender(UIRenderContext context) => Current?.Prerender(context);
    }
}
