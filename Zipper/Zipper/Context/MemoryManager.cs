using System;
using System.Runtime;
using System.Threading;

namespace Zipper.Context
{
    /// <summary>
    /// The memory manager class. Memory check is not thread safe. 
    /// The purpuse is to minimise OOM exceptions, not no elimenate them.
    /// </summary>
    public class MemoryManager : IMemoryManager
    {
        private readonly int _sleepLength = 0;

        public MemoryManager() : this(500) { }

        public MemoryManager(int sleepLength) => _sleepLength = sleepLength;

        /// <summary>
        /// Gets array of a given length, sleeps the thread if there is not enough memory
        /// All the allocation of less than 10mb, are treated as 10mb
        /// </summary>
        /// <param name="length">byte array length</param>
        /// <returns>Empty byte array</returns>
        public byte[] GetArrayWhenHasMemory(int length)
        {
            var memSizeInMb = GetMemSizeInMB(length);

            while (true)
            {
                try
                {
                    using (new MemoryFailPoint(memSizeInMb))
                    {
                        return new byte[length];
                    }
                }
                catch (InsufficientMemoryException)
                {
                    Thread.Sleep(_sleepLength);
                }
            }
            throw new OutOfMemoryException();
        }

        public void SleepThreadUntilHasMemory(int length)
        {
            var memSizeInMb = GetMemSizeInMB(length);

            while (true)
            {
                try
                {
                    using (new MemoryFailPoint(memSizeInMb))
                    {
                        return;
                    }
                }
                catch (InsufficientMemoryException)
                {
                    Thread.Sleep(_sleepLength);
                }
            }
        }

        private static int GetMemSizeInMB(int length)
        {
            var memSizeInMb = (int)Math.Ceiling(((double)length / ByteSize.Mega));
            memSizeInMb = memSizeInMb < 10 ? 10 : memSizeInMb;
            return memSizeInMb;
        }
    }
}
