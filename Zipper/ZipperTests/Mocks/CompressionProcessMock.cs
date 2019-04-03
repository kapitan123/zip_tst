using Zipper.Chunk;
using Zipper.Zipping.Processes;

namespace ZipperTests.Mocks
{
    class NullifyingProcess : IProcess
    {
        public IChunk ManipulateData(IChunk chunk) => new FileChunk(new byte[0], chunk.OrderNumber);
    }
}
