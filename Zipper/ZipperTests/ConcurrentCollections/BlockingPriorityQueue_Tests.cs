using Xunit;
using Zipper.ConcurrentCollections;
using Zipper.Chunk;
using System.Collections.Generic;

namespace ZipperTests.ConcurrentCollections
{
    public class BlockingPriorityQueue_Tests
    {
        [Fact]
        public void Take_Returns_Element_In_Ordered_Sequence_Regardless_Of_Adding_Order()
        {
            var q = new BlockingPriorityQueue();

            var addingOrder = new List<int>() { 9, 2, 3, 0 };

            addingOrder.ForEach(o => q.TryAdd(new FileChunk(new byte[0], o)));

            q.TryTake(out var firstPoped);
            q.TryTake(out var secondPoped);
            q.TryTake(out var thirdPoped);
            q.TryTake(out var fourthPoped);

            Assert.Equal(0, firstPoped.OrderNumber);
            Assert.Equal(2, secondPoped.OrderNumber);
            Assert.Equal(3, thirdPoped.OrderNumber);
            Assert.Equal(9, fourthPoped.OrderNumber);
        }
    }
}
