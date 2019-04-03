using System;
using System.IO;
using System.IO.Compression;
using Zipper.Chunk;
using Zipper.Context;

namespace Zipper.Zipping.Processes
{
    public class GZipCompress : IProcess
    {
        private byte[] headerPlaceHolder = new byte[sizeof(int)];

        private IMemoryManager _memManager;

        public GZipCompress(IMemoryManager memManager)
        {
            _memManager = memManager;
        }

        public IChunk ManipulateData(IChunk chunk)
        {
            WaitForMemory(chunk);

            var compressedContent = Compress(chunk.Content);

            WriteLengthToHeader(compressedContent);

            chunk.Content = compressedContent;
            return chunk;
        }

        private static void WriteLengthToHeader(byte[] compressedContent)
        {
            var zippedLeghtHeader = BitConverter.GetBytes(compressedContent.Length - sizeof(int));
            zippedLeghtHeader.CopyTo(compressedContent, 0);
        }

        private byte[] Compress(byte[] inputData)
        {
            using (var outStream = new MemoryStream())
            {
                using (var compressingStream = new BufferedStream(new GZipStream(outStream, CompressionMode.Compress)))
                {
                    outStream.Write(headerPlaceHolder, 0, headerPlaceHolder.Length);
                    compressingStream.Write(inputData, 0, inputData.Length);
                }

                return outStream.ToArray();
            }
        }

        private void WaitForMemory(IChunk chunk) => _memManager.SleepThreadUntilHasMemory(chunk.Length * 2);
    }
}
