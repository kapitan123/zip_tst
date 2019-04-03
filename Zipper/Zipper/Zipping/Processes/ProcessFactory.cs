using System;
using System.Collections.Generic;
using Zipper.Context;

namespace Zipper.Zipping.Processes
{
    public static class ProcessFactory
    {
        private static Dictionary<string, Func<IProcess>> _implementations = new Dictionary<string, Func<IProcess>>
        {
            ["compress"] = () => new GZipCompress(new MemoryManager()),
            ["decompress"] = () => new GZipDecompress(new MemoryManager()),
        };

        public static IProcess GetProcess(string processCode) => _implementations[processCode]();
    }
}
