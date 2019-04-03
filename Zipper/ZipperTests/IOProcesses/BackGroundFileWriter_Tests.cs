using System.Collections.Concurrent;
using Xunit;
using Zipper.IOProcesses;
using Zipper.Chunk;
using System;
using System.Linq;
using System.Collections.Generic;
using Zipper.Extensions;
using ZipperTests.Helpers;

namespace ZipperTests.IOProcesses
{
    public class BackGroundFileWriter_Tests: BaseBackGroundProcess_Tests
    {
        private BlockingCollection<IChunk> _processedQueue = new BlockingCollection<IChunk>();

        [Fact]
        public void Write_Concurrent_Does_Not_Corrupt_Data()
        {
            InitializeInput(randommizeOrder: false);

            new BackGroundFileWriter(_processedQueue, FileSystemMock)
                .Write(DefaultPath);

            var writtenData = ReadDefaultFileContent();

            Assert.Equal(GenFile.Content, writtenData);
        }

        [Fact]
        public void Write_NonSequential_Does_Not_Corrupt_Data()
        {
            InitializeInput(randommizeOrder: true);

            new BackGroundFileWriter(_processedQueue, FileSystemMock)
                .Write(DefaultPath);

            var writtenData = ReadDefaultFileContent();

            Assert.Equal(GenFile.Content, writtenData);
        }

        private void InitializeInput(bool randommizeOrder = false)
        {
            var chunks = ByteArrayHelper.ArrayToChunks(GenFile.Content, Config.ChunkSize);

            if (randommizeOrder)
            {
                FillQueueInRandomOrder(chunks);
            }
            else
            {
                _processedQueue.AddRange(chunks);
            }
            _processedQueue.CompleteAdding();
        }

        private void FillQueueInRandomOrder(IList<IChunk> chunks)
        {
            while (chunks.Count() > 0)
            {
                PopRandomElementToQueue(chunks);
            }
        }

        private void PopRandomElementToQueue(IList<IChunk> chunksList)
        {
            var randomIndex = GetRandomIndex(chunksList.Count);
            _processedQueue.Add(chunksList[randomIndex]);
            chunksList.RemoveAt(randomIndex);
        }

        private int GetRandomIndex(int limit) => new Random().Next(limit);

        private byte[] ReadDefaultFileContent() => FileSystemMock.File.ReadAllBytes(DefaultPath);
    }
}
