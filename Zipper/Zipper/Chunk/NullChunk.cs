namespace Zipper.Chunk
{
    public class NullChunk : IChunk
    {
        public byte[] Content { get; set; }

        public int Length { get; }

        public int OrderNumber { get; set; }

        public bool IsEmpty => Length == 0;

        public NullChunk()
        {
            OrderNumber = -1;
            Length = 0;
            Content = new byte[0];
        }

        public int CompareTo(IChunk other) => OrderNumber == other.OrderNumber ? 0 : OrderNumber > other.OrderNumber ? 1 : -1;

        public int CompareTo(object obj) => CompareTo((IChunk)obj);
    }
}
