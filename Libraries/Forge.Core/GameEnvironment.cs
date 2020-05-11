using System;
using System.Collections.Generic;
using System.Text;

namespace Forge.Core
{
    /// <summary>
    /// Represents the environment in which the game is running.
    /// </summary>
    public class GameEnvironment
    {
        public GameEnvironment(bool graphicsSupport)
        {
            GraphicsSupport = graphicsSupport;
        }

        public bool GraphicsSupport { get; }
    }
}
