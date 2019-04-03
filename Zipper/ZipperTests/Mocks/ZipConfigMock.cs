using Zipper.Context;

namespace ZipperTests.Mocks
{
    public class ZipConfigMock : IConfiguration
    {
        public int ThreadsCount => 1;

        public int ChunkSize => 1 * ByteSize.Mega;
    }
}
