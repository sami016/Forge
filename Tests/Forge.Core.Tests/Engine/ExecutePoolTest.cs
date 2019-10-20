using FluentAssertions;
using Forge.Core.Engine;
using System;
using System.Linq;
using System.Threading;
using Xunit;

namespace Forge.Core.Tests.Engine
{
    public class ExecutePoolTest : IThreadExecuteJob
    {
        private readonly ExecutePool _executePool;
        private bool[] _executedFlags;
        private int[] _threadIds;

        public ExecutePoolTest()
        {
            _executePool = new ExecutePool(8, this);
            _executedFlags = new bool[8];
            _threadIds = new int[8];
        }

        public void Execute(int threadNumber)
        {
            _executedFlags[threadNumber] = true;
            _threadIds[threadNumber] = Thread.CurrentThread.ManagedThreadId;
        }

        /// <summary>
        /// Check that the job is run for every thread, and once per thread.
        /// </summary>
        [Fact]
        public void CheckAllJobsRun()
        {
            _executePool.RunStep();

            for (var i=0; i<8; i++)
            {
                _executedFlags[i].Should().BeTrue();
            }

            _threadIds.Distinct().Count().Should().Be(8);
        }
    }
}
