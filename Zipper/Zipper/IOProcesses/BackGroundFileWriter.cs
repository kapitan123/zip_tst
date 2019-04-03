using System;
using System.Collections.Concurrent;
using System.IO;
using System.IO.Abstractions;
using Zipper.Chunk;
using Zipper.Extensions;
using Zipper.ConcurrentCollections;

namespace Zipper.IOProcesses
{
    public class BackGroundFileWriter
    {
        private readonly IFileSystem _fileSystem;

        public BlockingCollection<IChunk> InputQueue { get; }

        private SortedChunksQueue _postponded = new SortedChunksQueue(false);

        private int _nextExpectedChunk = 0;

        public BackGroundFileWriter(BlockingCollection<IChunk> inputQueue, IFileSystem fileSystem)
        {
            InputQueue = inputQueue;

            _fileSystem = fileSystem;
        }

        public void Write(string fileName)
        {
            using (var fileStream = _fileSystem.FileStream.Create(fileName, FileMode.Append, FileAccess.Write))
            {
                foreach (var chunk in InputQueue.GetConsumingEnumerable())
                {
                    if (IsNext(chunk))
                    {
                        AppendToStream(fileStream, chunk);
                    }
                    else
                    {
                        AddToPostponded(chunk);
                    }

                    WriteNextFromPostponded(fileStream);
                }
            }
        }

        private void WriteNextFromPostponded(Stream fileStream)
        {
            var firstinLine = _postponded.GetFirstOrDefault();
            while (firstinLine.OrderNumber == _nextExpectedChunk)
            {
                _postponded.RemoveFirst();
                AppendToStream(fileStream, firstinLine);
                firstinLine = _postponded.GetFirstOrDefault();
            }
        }

        private void AppendToStream(Stream fileStream, IChunk chunk)
        {
            fileStream.Append(chunk);
            _nextExpectedChunk++;
        }

        private void AddToPostponded(IChunk chunk) => _postponded.Add(chunk);

        private bool IsNext(IChunk chunk) => chunk.OrderNumber == _nextExpectedChunk;
    }
}
