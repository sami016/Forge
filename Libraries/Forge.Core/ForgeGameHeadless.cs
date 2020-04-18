using Forge.Core.Engine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace Forge.Core
{
    public class ForgeGameHeadless : IDisposable
    {
        private readonly ForgeEngine _engine;

        public ForgeGameHeadless(ForgeEngine engine)
        {
            _engine = engine;
            
            _engine.Initialise(null, null, null);
        }

        public void Start()
        {
            var thread = new Thread(Run);
            thread.Priority = ThreadPriority.Highest;
            thread.Start();
        }

        public void Run()
        {
            Stopwatch totalElapsedStopwatch = new Stopwatch();
            Stopwatch tickElapsedStopwatch = new Stopwatch();

            var timeDiff = TimeSpan.FromSeconds(0);
            var totalTime = TimeSpan.FromSeconds(0);
            totalElapsedStopwatch.Start();
            while (true)
            {
                tickElapsedStopwatch.Restart();
                tickElapsedStopwatch.Start();
                _engine.Tick(new GameTime(totalTime, timeDiff));
                tickElapsedStopwatch.Stop();
                timeDiff = tickElapsedStopwatch.Elapsed;
                totalTime = totalElapsedStopwatch.Elapsed;
            }
        }

        public void Dispose()
        {
        }
    }
}
