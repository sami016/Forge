using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Forge.UI.Glass
{
    public class UIInitialiseContext
    {
        public GraphicsDevice GraphicsDevice { get; }

        private readonly IServiceProvider _serviceProvider;

        public UIInitialiseContext(GraphicsDevice graphicsDevice, IServiceProvider serviceProvider)
        {
            GraphicsDevice = graphicsDevice;
            _serviceProvider = serviceProvider;
        }

        public T GetService<T>()
        {
            return _serviceProvider.GetService<T>();
        }
    }
}
