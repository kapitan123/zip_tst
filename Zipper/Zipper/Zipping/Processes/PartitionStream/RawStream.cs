using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zipper.Chunk;
using Zipper.Context;
using Zipper.Extensions;
using Zipper.Zipping.Processes.ChunkableStream;

namespace Zipper.Zipping.Processes.PartitionStream
{
    public class RawStream : IPartitionStream
    {
        private int _chunkSize;

        private Stream _stream;

        public int NextChunkSize => _chunkSize;

        private IMemoryManager _memManager;

        public RawStream(IMemoryManager memManager, int chunkSize)
        {
            _chunkSize = chunkSize;
            _memManager = memManager;
        }

        public IEnumerator<IChunk> GetEnumerator()
        {
            var chunk = ReadChunkWithMemoryCheck();
            while (!chunk.IsEmpty)
            {
                yield return chunk;
                chunk = ReadChunkWithMemoryCheck();
            }
        }

        private IChunk ReadChunkWithMemoryCheck()
        {
            var buffer = _memManager.GetArrayWhenHasMemory(_chunkSize);
            var chunk = _stream.ReadChunk(_chunkSize, buffer);
            return chunk;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void SetSource(Stream sourceStream) => _stream = sourceStream;
    }
}
