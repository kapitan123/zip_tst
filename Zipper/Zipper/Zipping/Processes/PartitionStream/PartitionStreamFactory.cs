using System;
using System.Collections.Generic;
using Zipper.Context;
using Zipper.Zipping.Processes.ChunkableStream;

namespace Zipper.Zipping.Processes.PartitionStream
{
    public class PartitionStreamFactory
    {
        private static Dictionary<string, Func<IConfiguration, IPartitionStream>> _implementations 
            = new Dictionary<string, Func<IConfiguration,IPartitionStream>>
        {
            ["compress"] = (config) => new RawStream(new MemoryManager(), config.ChunkSize),
            ["decompress"] = (config) => new GZipCompressedStream(new MemoryManager()),
        };

        public static IPartitionStream GetStream(string processCode, IConfiguration config) => _implementations[processCode](config);
    }
}
