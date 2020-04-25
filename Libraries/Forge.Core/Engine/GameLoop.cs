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
            var tickContext = new TickContext
            {
                DeltaTime = (uint)gameTime.ElapsedGameTime.Ticks,
                DeltaTimeSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds,
                TimeSeconds = (float)gameTime.TotalGameTime.TotalSeconds,
                ServiceProvider = null,
                TimeStamp = (uint)gameTime.TotalGameTime.Ticks
            };
            foreach (var component in _entityManager.GetAll<IPreTick>())
            {
                component.PreTick(tickContext);
            }
            foreach (var component in _entityManager.GetAll<ITick>())
            {
                component.Tick(tickContext);
            }
            foreach (var component in _entityManager.GetAll<IPostTick>())
            {
                component.PostTick(tickContext);
            }
        }
    }
}
