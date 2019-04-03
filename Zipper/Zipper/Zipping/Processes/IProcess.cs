using System.IO;
using Zipper.Chunk;
using Zipper.Zipping.Processes.ChunkableStream;

namespace Zipper.Zipping.Processes
{
    public interface IProcess
    {
        IChunk ManipulateData(IChunk chunk);
    }
}