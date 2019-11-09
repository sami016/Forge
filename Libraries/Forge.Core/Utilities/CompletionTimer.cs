using System;
using System.Collections.Generic;
using System.Text;

namespace Forge.Core.Utilities
{
    public class CompletionTimer
    {
        private readonly long _target;
        private long _current;

        public CompletionTimer(TimeSpan timeSpan)
        {
            _target = timeSpan.Ticks;
            _current = 0;
        }

        public void Restart()
        {
            _current = 0;
        }
        
        public void Tick(uint elapsedTicks)
        {
            _current += elapsedTicks;
        }

        public bool Completed => _current >= _target;

        public TimeSpan RemainingTime => TimeSpan.FromTicks(_target - _current);
    }
}
