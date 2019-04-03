using System.Collections.Generic;
using System.Linq;
using Zipper.Extensions;
using Zipper.Chunk;

namespace ZipperTests.Helpers
{
    public static class ByteArrayHelper
    {
        public static byte[] ChunkCollectionToByteArray(IEnumerable<IChunk> chunks)
        {
            var result = new List<byte>();
            foreach (var chunk in chunks)
            {
                var list = new List<byte>(chunk.Content);
                result.AddRange(list);
            }
            return result.ToArray();
        }

        public static IList<IChunk> ArrayToChunks(byte[] content, int chunkSize)
        {
            var orderNumber = 0;
            return content
                .Split(chunkSize)
                .Select(byteArr => (IChunk)new FileChunk(byteArr.ToArray(), orderNumber++))
                .ToList();
        }

        public static bool AreEqual(byte[] ar1, byte[] ar2)
        {
            return ar1.SequenceEqual(ar2);
        }
    }
}
