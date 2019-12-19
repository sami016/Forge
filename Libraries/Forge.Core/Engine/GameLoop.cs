using Forge.Core.Interfaces;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Forge.Core.Engine
{
    /// <summary>
    /// Executes the core game loop on a thread by thread basis.
    /// </summary>
    public class GameLoop
    {
        private readonly EntityManager _entityManager;

        public GameTime GameTime { get; set; }

        public GameLoop(EntityManager entityManager)
        {
            _entityManager = entityManager;
        }

        public void Execute()
        {
            var gameTime = GameTime;
            foreach (var tick in _entityManager.GetAll<ITick>())
            {
                // To do context.
                tick.Tick(new TickContext
                {
                    DeltaTime = (uint)gameTime.ElapsedGameTime.Ticks,
                    DeltaTimeSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds,
                    ServiceProvider = null,
                    TimeStamp = (uint)gameTime.TotalGameTime.Ticks
                });
            }
        }
    }
}
