using System.IO;
using System.IO.Compression;
using Zipper.Chunk;
using Zipper.Context;

namespace Zipper.Zipping.Processes
{
    public class GZipDecompress : IProcess
    {
        private IMemoryManager _memManager;

        public GZipDecompress(IMemoryManager memManager)
        {
            _memManager = memManager;
        }

        public IChunk ManipulateData(IChunk chunk)
        {
            WaitForMemory(chunk);

            chunk.Content = Decompress(chunk.Content);
            return chunk;
        }

        private byte[] Decompress(byte[] inputData)
        {
            using (var decompressedStream = new MemoryStream())
            {
                using (var compressedStream = new MemoryStream(inputData))
                using (var decompressionStream = new BufferedStream(new GZipStream(compressedStream, CompressionMode.Decompress), inputData.Length))
                {
                    decompressionStream.CopyTo(decompressedStream);
                }
                return decompressedStream.ToArray();
            }
        }

        private void WaitForMemory(IChunk chunk) => _memManager.SleepThreadUntilHasMemory(chunk.Length * 2);
    }
}
