using System.Collections.Concurrent;
using Xunit;
using Zipper.Chunk;
using System.Linq;
using Zipper.Zipping.Pool;
using ZipperTests.Mocks;
using Zipper.Zipping.Processes;
using ZipperTests.Helpers;
using ZipperTests.IOProcesses;

namespace ZipperTests.Zipping
{
    public class ZippingOrchestara_Tests: BaseBackGroundProcess_Tests
    {
        private BlockingCollection<IChunk> _inQueue = new BlockingCollection<IChunk>();

        private BlockingCollection<IChunk> _outQueue = new BlockingCollection<IChunk>();

        private IProcess _nullifyingProcess = new NullifyingProcess();

        public ZippingOrchestara_Tests()
        {
            FillInputQueue();
        }

        [Fact]
        public void ProcessQueue_Performs_Operation_On_All_Elements_From_InQueue_To_OutQueue()
        {
            new ZippingPool(_inQueue, _outQueue, _nullifyingProcess)
                .ProcessQueue(Config.ThreadsCount);

            var hasNonEmptyChunks = _outQueue.Any(c => c.Content.Length > 0);

            Assert.False(hasNonEmptyChunks);
        }

        [Fact]
        public void ProcessQueue_Transfers_All_Elements_To_Out()
        {
            var inLengthBeforeProcess = _inQueue.Count;

            new ZippingPool(_inQueue, _outQueue, _nullifyingProcess)
                .ProcessQueue(Config.ThreadsCount);

            var outLengthAfterProcess = _outQueue.Count;

            Assert.Equal(inLengthBeforeProcess, outLengthAfterProcess);
        }

        [Fact]
        public void ProcessQueue_Empties_Input_Queue()
        {
            new ZippingPool(_inQueue, _outQueue, _nullifyingProcess)
                .ProcessQueue(Config.ThreadsCount);

            var inLengthAfterProcess = _inQueue.Count;

            Assert.Equal(0, inLengthAfterProcess);
        }

        private void FillInputQueue()
        {
            ByteArrayHelper.ArrayToChunks(GenFile.Content, Config.ChunkSize);
            _inQueue.CompleteAdding();
        }
    }
}
