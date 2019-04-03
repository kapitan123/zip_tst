
namespace Zipper.Context
{
    public interface IMemoryManager
    {
        byte[] GetArrayWhenHasMemory(int byteArrayLegth);

        void SleepThreadUntilHasMemory(int length);
    }
}
