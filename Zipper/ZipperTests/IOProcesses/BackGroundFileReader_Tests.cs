using System.Collections.Concurrent;
using Xunit;
using Zipper.IOProcesses;
using Zipper.Chunk;
using ZipperTests.Helpers;
using ZipperTests.Mocks;
using Zipper.Zipping.Processes.ChunkableStream;

namespace ZipperTests.IOProcesses
{
    public class BackGroundFileReader_Tests : BaseBackGroundProcess_Tests
    {
        private BlockingCollection<IChunk> _rawQueue = new BlockingCollection<IChunk>();
        private IPartitionStream _partitionStreamMock;
        public BackGroundFileReader_Tests()
        {
            GenFile.Create(FileSystemMock, DefaultPath);
            _partitionStreamMock = new PartitionStreamMock(Config.ChunkSize);
        }

        [Fact]
        public void ReadInChunks_File_Produces_Corresponding_QueeOfChunks()
        {
            new BackGroundFileReader(_rawQueue, FileSystemMock, _partitionStreamMock)
                .ReadInChunks(GenFile.Path);

            var chunksToExpect = GetChunksCountFromFileLength();

            Assert.Equal(chunksToExpect, _rawQueue.Count);
        }

        [Fact]
        public void ReadInChunks_Does_Not_Corrupt_Content()
        {
            new BackGroundFileReader(_rawQueue, FileSystemMock, _partitionStreamMock)
                .ReadInChunks(GenFile.Path);

            var restoredByteArray = ByteArrayHelper.ChunkCollectionToByteArray(_rawQueue);
            var _inputIsEqualToOutPut = ByteArrayHelper.AreEqual(GenFile.Content, restoredByteArray);

            Assert.True(_inputIsEqualToOutPut);
        }

        [Fact]
        public void ReadInChunks_MemoryConstraints_Does_Not_Corrupt_Content()
        {
            new BackGroundFileReader(_rawQueue, FileSystemMock, _partitionStreamMock)
                .ReadInChunks(GenFile.Path);

            var restoredByteArray = ByteArrayHelper.ChunkCollectionToByteArray(_rawQueue);
            var _inputIsEqualToOutPut = ByteArrayHelper.AreEqual(GenFile.Content, restoredByteArray);

            Assert.True(_inputIsEqualToOutPut);
        }

        private int GetChunksCountFromFileLength() => (GenFile.Length + Config.ChunkSize - 1) / Config.ChunkSize;
    }
}
