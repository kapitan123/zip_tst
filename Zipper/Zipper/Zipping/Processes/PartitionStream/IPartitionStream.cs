using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zipper.Chunk;

namespace Zipper.Zipping.Processes.ChunkableStream
{
    public interface IPartitionStream : IEnumerable<IChunk>
    {
        void SetSource(Stream sourceStream);

        int NextChunkSize { get; }
    }
}
