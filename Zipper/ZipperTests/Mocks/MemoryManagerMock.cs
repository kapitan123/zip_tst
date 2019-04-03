using Zipper.Context;

namespace ZipperTests.Mocks
{
    class MemoryManagerMock : IMemoryManager
    {
        public byte[] GetArrayWhenHasMemory(int byteArrayLegth) => new byte[byteArrayLegth];


        public void SleepThreadUntilHasMemory(int length) 
        {
            return;
        }
    }
}
