using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Forge.Core.Engine
{
    /// <summary>
    /// Responsible for maintaining update actions for each thread.
    /// These update actions encapsulate state changes for a particular thread context, including side effects.
    /// </summary>
    public class SideEffectManager
    {

        private IDictionary<int, ConcurrentQueue<Action>> _updatesByThread = new Dictionary<int, ConcurrentQueue<Action>>();

        public SideEffectManager(int threadCount)
        {
            _updatesByThread = new Dictionary<int, ConcurrentQueue<Action>>();
            for (var i=0; i<threadCount; i++)
            {
                _updatesByThread[i] = new ConcurrentQueue<Action>();
            }
        }

        /// <summary>
        /// Enqueue an update action.
        /// </summary>
        /// <param name="threadNumber">thread context number</param>
        /// <param name="updateAction">update action</param>
        public void Enqueue(int threadNumber, Action updateAction)
        {
            _updatesByThread[threadNumber].Enqueue(updateAction);
        }

        /// <summary>
        /// Get all updates for a thread context number.
        /// </summary>
        /// <param name="threadNumber">thread context number</param>
        /// <returns>all update actions</returns>
        public IEnumerable<Action> DequeueAll(int threadNumber)
        {
            var queue = _updatesByThread[threadNumber];
            var result = queue.ToArray();
            // Clear the queue.
            while (queue.TryDequeue(out var _))
            {
            }

            return result;
        }
    }
}
