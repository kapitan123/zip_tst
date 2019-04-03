using System;
using System.IO;

namespace Zipper.Context
{
    public static class ZipConfigurator
    {
        public static IConfiguration DefaultConfig { get; }

        static ZipConfigurator()
        {
            DefaultConfig = new ZipConfig(10 * ByteSize.Mega, Environment.ProcessorCount);
        }

        public static IConfiguration GetConfigForAFile(int chunkSize, string filePath)
        {
            if (chunkSize <= 0) throw new ArgumentOutOfRangeException(nameof(chunkSize));

            var processorsCount = Environment.ProcessorCount;

            var filePartition = Math.Ceiling((double)new FileInfo(filePath).Length / chunkSize);

            var threads = (int)Math.Min(filePartition, processorsCount);

            return new ZipConfig(chunkSize, threads);
        }

        private class ZipConfig : IConfiguration
        {
            public int ThreadsCount { get; }

            public int ChunkSize { get; }

            public ZipConfig(int chunkSize, int threadsCount)
            {
                ChunkSize = chunkSize;
                ThreadsCount = threadsCount;
            }
        }
    }

}
