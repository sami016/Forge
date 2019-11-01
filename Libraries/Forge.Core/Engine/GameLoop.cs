using Forge.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Forge.Core.Engine
{
    /// <summary>
    /// Executes the core game loop on a thread by thread basis.
    /// </summary>
    public class GameLoop : IThreadExecuteJob
    {
        private readonly EntityManager _entityManager;
        private readonly SideEffectManager _sideEffectManager;

        public GameLoopPhase Phase { get; set; }

        public GameLoop(EntityManager entityManager, SideEffectManager SideEffectManager)
        {
            _entityManager = entityManager;
            _sideEffectManager = SideEffectManager;
        }

        public void Execute(int threadNumber)
        {
            var poolShard = _entityManager.Pools.Shards[threadNumber];

            if (Phase == GameLoopPhase.Tick)
            {
                foreach (var tick in poolShard.GetAll<ITick>())
                {
                    // To do context.
                    tick.Tick(null);
                }
            } 
            else if (Phase == GameLoopPhase.Update)
            {
                foreach (var updateAction in _sideEffectManager.DequeueAll(threadNumber))
                {
                    updateAction();
                }
            }
        }
    }
}
