using System;
using System.Collections.Concurrent;
using System.IO.Abstractions;
using Zipper.Chunk;
using Zipper.IOProcesses;
using Zipper.Zipping.Processes;
using Zipper.Zipping.Processes.ChunkableStream;

namespace Zipper.Zipping.Workers
{
    public class ReadWorker : Worker
    {
        protected override string Name => "Reader";

        private BlockingCollection<IChunk> _rawQueue;

        private string _inputFilePath;

        private IPartitionStream _partitionStream;

        public ReadWorker(BlockingCollection<IChunk> rawQueue, string inputFilePath, IPartitionStream partitionStream) : base()
        {
            _rawQueue = rawQueue;
            _inputFilePath = inputFilePath;
            _partitionStream = partitionStream;
        }

        protected override void DoWork()
        {
            try
            {
                new BackGroundFileReader(_rawQueue, new FileSystem(), _partitionStream).ReadInChunks(_inputFilePath);
            }
            catch (Exception e)
            {
                HasFailed = true;
                InternalException = e;
                _rawQueue.CompleteAdding();
            }
        }
    }
}
