using Zipper.Zipping.Processes.ChunkableStream;
using Zipper.Zipping.Processes.PartitionStream;

namespace ZipperTests.Mocks
{
    /// <summary>
    /// Raw stream is determetistic enough
    /// </summary>
    public class PartitionStreamMock : RawStream, IPartitionStream
    {
        public PartitionStreamMock(int chunkSize) : base(new MemoryManagerMock(), chunkSize)
        {
        }
    }
}
