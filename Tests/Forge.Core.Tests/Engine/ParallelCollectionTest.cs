using FluentAssertions;
using Forge.Core.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Forge.Core.Tests.Engine
{
    public class ParallelCollectionTest
    {
        ParallelCollection<object> ParallelCollection;

        object item1 = new object();

        public ParallelCollectionTest()
        {
        }

        [Fact]
        public void AllocatedIdsShouldBeUnique()
        {
            ParallelCollection = new ParallelCollection<object>(2, 10);
            var itemAddedCalls = 0;

            ParallelCollection.ItemAdded += (i, s) =>
            {
                itemAddedCalls++;
            };

            var ids = new List<uint>();
            for (var i=0; i<10; i++)
            {
                var id = ParallelCollection.Add(new object());
                ids.Add(id);
            }
            ids.Distinct().Count().Should().Be(10);
            // Once at capabity, should throw.
            Action addOneMore = () => ParallelCollection.Add(new object());
            addOneMore.Should().Throw<Exception>();
        }

        [Fact]
        public void ShouldDistributeEvenly()
        {
            ParallelCollection = new ParallelCollection<object>(5, 50);
            for (var i = 0; i < 5; i++)
            {
                ParallelCollection.Add(new object());
            }
            for (var i = 0; i < 5; i++)
            {
                ParallelCollection.Sets[i]
                    .Where(x => x != null)
                    .Count()
                    .Should()
                    .Be(1);
            }
        }
    }
}
