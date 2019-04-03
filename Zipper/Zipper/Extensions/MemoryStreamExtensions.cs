using System.IO;
using System.Linq;
using Zipper.Chunk;

namespace Zipper.Extensions
{
    public static class MemoryStreamExtensions
    {
        public static void Append(this Stream to, IChunk chunk)
        {
            to.Write(chunk.Content, 0, chunk.Length);
        }

        public static IChunk ReadChunk(this Stream stream, int chunkSize, byte[] buffer)
        {
            var bytesRead = stream.Read(buffer, 0, chunkSize);

            if (bytesRead < chunkSize)
            {
                buffer = buffer.Take(bytesRead).ToArray();
            }

            return ChunkFactory.GetNew(buffer, 0);
        }
    }
}
