using Forge.UI.Glass.Elements;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Forge.UI.Glass.Templates
{
    public interface ITemplate : IDisposable, IElement
    {
        /// <summary>
        /// The latest evaluated value of the template.
        /// </summary>
        IElement Current { get; set; }

        /// <summary>
        /// Called when the element template is initialised.
        /// </summary>
        void PreEvaluate();

        /// <summary>
        /// Evaluate the template into a component.
        /// This component can subsequently be rendered to the screen.
        /// </summary>
        /// <returns></returns>
        IElement Evaluate();

        /// <summary>
        /// Makes the template render again.
        /// </summary>
        void Reevaluate();

    }
}
