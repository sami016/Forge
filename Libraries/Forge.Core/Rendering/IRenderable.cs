using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Forge.Core.Rendering
{
    public interface IRenderable
    {
        void Render(GameTime gameTime);
    }
}
