using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using Zipper.Chunk;
using Zipper.Context;
using Zipper.Zipping.Processes;

namespace Zipper.Zipping.Pool
{
    public class ZippingPool
    {
        private List<Thread> _workers = new List<Thread>();

        private int _completeWorkers = 0;

        public BlockingCollection<IChunk> RawQueue { get; }

        public BlockingCollection<IChunk> ProcessedQueue { get; }

        public IProcess Process { get; }

        public bool PoolHasFailed { get; private set; }

        private Exception _poolException = null;
        public Exception PoolException { get => _poolException; private set => _poolException = value; }

        public ZippingPool(BlockingCollection<IChunk> rawQueue, BlockingCollection<IChunk> processedQueue, IProcess process)
        {
            RawQueue = rawQueue;
            ProcessedQueue = processedQueue;
            Process = process;
        }

        public void ProcessQueue(int threadCount)
        {
            LaunchThreads(threadCount);
            ThrowIfAnyFailed();
        }

        private void ThrowIfAnyFailed()
        {
            if (PoolHasFailed)
            {
                throw PoolException;
            }
        }

        private void LaunchThreads(int threadCount)
        {
            for (var i = 0; i < threadCount; i++)
            {
                var newThread = GetNewZippingThread(i);
                _workers.Add(newThread);
                newThread.Start();
            }

            _workers.ForEach(t => t.Join());
        }

        private void ExecuteProcess()
        {
            try
            {
                HandleQueue();
                IncreaseCompleteCounter();

                if (AllAreWorkersComplete())
                {
                    ProcessedQueue.CompleteAdding();
                }
            }
            catch (Exception exception)
            {
                SetPoolAsFailed(exception);
            }
        }

        private void SetPoolAsFailed(Exception exception)
        {
            // We only need the first error which led us to failure
            Interlocked.CompareExchange(ref _poolException, exception, null);
            ProcessedQueue.CompleteAdding();
            PoolHasFailed = true;
        }

        private void HandleQueue()
        {
            foreach (var chunk in RawQueue.GetConsumingEnumerable())
            {
                AddProcessedChunkToOutPut(chunk);
            }
        }

        private Thread GetNewZippingThread(int number) => new Thread(() => ExecuteProcess())
        {
            Name = $"Zipping Pool Thread {number}",
            IsBackground = true
        };

        private void IncreaseCompleteCounter() => Interlocked.Increment(ref _completeWorkers);

        private void AddProcessedChunkToOutPut(IChunk chunk)
        {
            ProcessedQueue.Add(Process.ManipulateData(chunk));
        }

        private bool AllAreWorkersComplete() => _completeWorkers == _workers.Count;
    }
}
