using System;
using System.Collections.Concurrent;
using Zipper.Zipping.Pool;
using Zipper.Chunk;
using Zipper.Zipping.Processes;
using Zipper.Context;

namespace Zipper.Zipping.Workers
{
    public class ZipWorker : Worker
    {
        protected override string Name => "Zipper";

        private BlockingCollection<IChunk> _processedQueue;

        private BlockingCollection<IChunk> _rawQueue;

        private IProcess _process;
        private int _threadsCount = 1;

        public ZipWorker(BlockingCollection<IChunk> processedQueue, BlockingCollection<IChunk> rawQueue, IProcess process, int threadsCount)
        {
            _processedQueue = processedQueue;
            _rawQueue = rawQueue;
            _process = process;
            _threadsCount = threadsCount;
        }

        protected override void DoWork()
        {
            try
            {
                new ZippingPool(_rawQueue, _processedQueue, _process).ProcessQueue(_threadsCount);
            }
            catch (Exception e)
            {
                HasFailed = true;
                InternalException = e;
                _processedQueue.CompleteAdding();
            }
        }

    }
}
