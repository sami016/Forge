using Forge.Core.Interfaces;
using Forge.UI.Glass.Interaction;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Forge.UI.Glass.Elements
{
    public interface IElement : ITick
    {
        IList<IElement> Children { get; }
        Rectangle Position { get; set; }
        UIEvents Events { get; }
        Action<IElement> Init { get; set; }
        void Render(UIRenderContext context);
        void Prerender(UIRenderContext context);

        /// <summary>
        /// Used to initialises elements.
        /// </summary>
        /// <param name="initialiseContext"></param>
        void Initialise(UIInitialiseContext initialiseContext);

    }
}
