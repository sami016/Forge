using Forge.Core.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Forge.Core.Rendering
{
    public class WindowControl : Component
    {
        [Inject] GraphicsDeviceManager GraphicsDeviceManager { get; set; }
        [Inject] GraphicsDevice GraphicsDevice { get; set; }

        public void AutoSizeToScreen()
        {
            // Take the adapter with the widest area, prefer wide screen.
            var adapter = GraphicsAdapter.Adapters
                .OrderByDescending(x => (x.IsWideScreen ? 99999 : 0) + x.CurrentDisplayMode.Width * x.CurrentDisplayMode.Height)
                .FirstOrDefault();

            GraphicsDeviceManager.PreferredBackBufferWidth = adapter.CurrentDisplayMode.Width;  // set this value to the desired width of your window
            GraphicsDeviceManager.PreferredBackBufferHeight = adapter.CurrentDisplayMode.Height;   // set this value to the desired height of your window
            GraphicsDeviceManager.ApplyChanges();
        }

        public void SetFullscreen()
        {

            if (!GraphicsDeviceManager.IsFullScreen)
            {
                GraphicsDeviceManager.ToggleFullScreen();
            }
        }
    }
}
