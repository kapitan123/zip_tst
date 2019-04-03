using System;

namespace Zipper.Chunk
{
    public interface IChunk : IComparable, IComparable<IChunk>
    {
        byte[] Content { get; set; }

        int Length { get; }

        int OrderNumber { get; set; }

        bool IsEmpty { get; }
    }
}