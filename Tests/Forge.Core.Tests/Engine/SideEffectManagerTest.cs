using FluentAssertions;
using Forge.Core.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Forge.Core.Tests.Engine
{
    public class SideEffectManagerTest
    {
        SideEffectManager SideEffectManager;

        [Fact]
        public void TestThreadIsolation()
        {
            SideEffectManager = new SideEffectManager(2);

            SideEffectManager.Enqueue(0, () => { });
            SideEffectManager.DequeueAll(1).Count().Should().Be(0);
            SideEffectManager.DequeueAll(0).Count().Should().Be(1);

            SideEffectManager.Enqueue(1, () => { });
            SideEffectManager.DequeueAll(1).Count().Should().Be(1);
            SideEffectManager.DequeueAll(0).Count().Should().Be(0);
        }

        [Fact]
        public void TestParallelEnqueue()
        {
            SideEffectManager = new SideEffectManager(1);
            var count = 0;

            for (var i=0; i<100; i++)
            {
                Task.Run(() =>
                {
                    SideEffectManager.Enqueue(0, () =>
                    {
                        count++;
                    });
                });
            }

            foreach (var action in SideEffectManager.DequeueAll(0))
            {
                action();
            }

            count.Should().Be(100);
        }
    }
}
