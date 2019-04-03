using System.Collections.Concurrent;
using System.IO;
using System.IO.Abstractions;
using Zipper.Chunk;
using Zipper.Zipping.Processes.ChunkableStream;

namespace Zipper.IOProcesses
{
    public class BackGroundFileReader
    {
        private readonly IFileSystem _fileSystem;

        private readonly IPartitionStream _partitionStream;

        public BlockingCollection<IChunk> OutputQueue { get; }

        private int _chunksRead = -1;

        public BackGroundFileReader(BlockingCollection<IChunk> outputQueue, IFileSystem fileSystem, IPartitionStream partitionStream)
        {
            OutputQueue = outputQueue;
            _fileSystem = fileSystem;
            _partitionStream = partitionStream;
        }

        public void ReadInChunks(string fileName)
        {
            try
            {
                AddWholeFileToTheQueue(fileName);
            }
            finally
            {
                FinishAddingToQueue();
            }
        }

        private void AddWholeFileToTheQueue(string fileName)
        {
            using (var fileStream = _fileSystem.FileStream.Create(fileName, FileMode.Open, FileAccess.Read))
            {
                _partitionStream.SetSource(fileStream);
                foreach (var chunk in _partitionStream)
                {
                    EnqueueChunk(chunk);
                }
            }
        }

        private void EnqueueChunk(IChunk chunk)
        {
            chunk.OrderNumber = ++_chunksRead;
            OutputQueue.Add(chunk);
        }

        private void FinishAddingToQueue() => OutputQueue.CompleteAdding();
    }
}
