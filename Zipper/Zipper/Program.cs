using System;
using System.Collections.Concurrent;
using Zipper.Chunk;
using Zipper.Zipping.Processes;
using Zipper.InputHandling;
using Zipper.Zipping.Workers;
using System.Collections.Generic;
using System.Linq;
using Zipper.Zipping.Processes.PartitionStream;
using Zipper.ConcurrentCollections;
using Zipper.Context;

namespace Zipper
{
    public class Program
    {
        public static int Main(string[] args)
        {
            var isError = 0;
            using (var processedQueue = new BlockingCollection<IChunk>(new BlockingPriorityQueue()))
            using (var rawQueue = new BlockingCollection<IChunk>(new BlockingPriorityQueue()))
            {
                try
                {
                    var arguments = Arguments.Parse(args);
                    var process = ProcessFactory.GetProcess(arguments.Command);
                    var config = ZipConfigurator.GetConfigForAFile(10 * ByteSize.Mega, arguments.InputFilePath);
                    var chunkableStream = PartitionStreamFactory.GetStream(arguments.Command, config);

                    var workers = new List<Worker>()
                    {
                        new ReadWorker(rawQueue, arguments.InputFilePath, chunkableStream),
                        new ZipWorker(processedQueue, rawQueue, process, config.ThreadsCount),
                        new WriteWorker(processedQueue, arguments.OutputFilePath)
                    };

                    workers.ForEach(w => w.Start());

                    workers.ForEach(w => w.Join());

                    if (workers.Any(w => w.HasFailed))
                    {
                        throw workers.First(w => w.HasFailed).InternalException;
                    }

                }
                catch (Exception e)
                {
                    ShowError(e);
                    isError = 1;
                }
            }
            Console.WriteLine(isError);
            return isError;
        }

        private static void ShowError(Exception e)
        {
            Console.Write($"\n An error has occurred: \n {e.Message}");
        }
    }
}
