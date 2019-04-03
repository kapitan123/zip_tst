using System;
using System.Collections.Concurrent;
using System.IO.Abstractions;
using Zipper.Chunk;
using Zipper.IOProcesses;

namespace Zipper.Zipping.Workers
{
    public class WriteWorker : Worker
    {
        protected override string Name => "Writer";

        private BlockingCollection<IChunk> _processedQueue;

        private string _outputFilePath;

        public WriteWorker(BlockingCollection<IChunk> processedQueue, string outputFilePath)
        {
            _processedQueue = processedQueue;
            _outputFilePath = outputFilePath;
            Thread.Priority = System.Threading.ThreadPriority.AboveNormal;
        }

        protected override void DoWork()
        {        
            try
            {
                new BackGroundFileWriter(_processedQueue, new FileSystem()).Write(_outputFilePath);
            }
            catch (Exception e)
            {
                HasFailed = true;
                InternalException = e;
            }
        }
    }
}
