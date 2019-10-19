using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Forge.Core.Engine
{
    /// <summary>
    /// A pool of threads for executing a predefined job.
    /// Each step run will block until all of the threads have completed running the job.
    /// </summary>
    public class ExecutePool : IDisposable
    {
        private readonly int _numThreads;
        private readonly IThreadExecuteJob _job;
        private readonly EventWaitHandle _finished = new AutoResetEvent(false);

        private bool _started;
        private bool _disposed;
        private int _currentIndex;
        private int _numCompleted;
        private EventWaitHandle[] _initiateEvents;

        public ExecutePool(int numThreads, IThreadExecuteJob job)
        {
            _numThreads = numThreads;
            _job = job;
            Start();
        }

        private void Start()
        {
            if (_started)
            {
                throw new Exception("Already started");
            }

            _started = true;
            _initiateEvents = new EventWaitHandle[_numThreads - 1];
            for (var i = 0; i < _numThreads - 1; i++)
            {
                _initiateEvents[i] = new AutoResetEvent(false);
                new Thread(Run)
                {
                    IsBackground = true,
                }.Start(_initiateEvents[i]);
            }
        }

        public void RunStep()
        {
            // Set current index to 0. 
            _currentIndex = 0;
            // Reset number completed.
            _numCompleted = 0;
            // This starts all the threads processing.
            foreach (var initiatedEvent in _initiateEvents)
            {
                initiatedEvent.Set();
            }
            // Run the first in the current thread.
            Process(0);
            // Wait for all threads to finish.
            _finished.WaitOne();
        }

        private void Run(object state)
        {
            var startEvent = (AutoResetEvent)state;
            while (!_disposed)
            {
                // Wait for initiation.
                startEvent.WaitOne();

                // increments the position atomically.
                var threadNumber = Interlocked.Increment(ref _currentIndex);

                Process(threadNumber);
            }
        }

        private void Process(int threadNumber)
        {

            // Perform the job on this thread, passing in the thread number.
            _job.Execute(threadNumber);

            // Increase the number of completed tasks. If this matches the number of threads, we're done.
            if (Interlocked.Increment(ref _numCompleted) == _numThreads)
            {
                _finished.Set();
            }
        }

        public void Dispose()
        {
            _disposed = true;
        }
    }
}
