using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zipper.Chunk;
using Zipper.Extensions;
using Zipper.Context;
using Zipper.Zipping.Processes.ChunkableStream;

namespace Zipper.Zipping.Processes.PartitionStream
{
    public class GZipCompressedStream : IPartitionStream
    {
        private Stream _stream;

        private byte[] _headerBuffer = new byte[sizeof(int)];

        public int NextChunkSize { get; private set; }

        private IMemoryManager _memManager;

        public GZipCompressedStream(IMemoryManager memManager)
        {
            _memManager = memManager;
        }

        public IEnumerator<IChunk> GetEnumerator()
        {
            var chunk = GetNextHeaderLenghtChunk();
            while (!chunk.IsEmpty)
            {
                yield return chunk;

                chunk = GetNextHeaderLenghtChunk();
            }
        }

        private IChunk GetNextHeaderLenghtChunk()
        {
            var currentDataPartSize = ReadNextChunkSize();
            if(currentDataPartSize == 0)
            {
                return ChunkFactory.GetNull();
            }

            var buffer = _memManager.GetArrayWhenHasMemory(currentDataPartSize);
            return _stream.ReadChunk(currentDataPartSize, buffer); ;
        }

        public int ReadNextChunkSize()
        {
            var bytesRead = _stream.Read(_headerBuffer, 0, sizeof(int));

            return bytesRead == 0 ? 0 : BitConverter.ToInt32(_headerBuffer, 0);
        }

        public void SetSource(Stream sourceStream) => _stream = sourceStream;

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
