using System;

namespace Zipper.Chunk
{
    public class FileChunk : IChunk
    {
        public byte[] Content { get; set; }

        public int Length => Content.Length;

        private int _orderNumber;
        public int OrderNumber
        {
            get => _orderNumber;
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("OrderNumber Can't be less than 0");
                }
                _orderNumber = value;
            }
        }

        public bool IsEmpty => Length == 0;

        public FileChunk(byte[] content) : this(content, 0)
        {
        }

        public FileChunk(byte[] content, int orderNumber)
        {
            Content = content;
            OrderNumber = orderNumber;
        }

        public int CompareTo(IChunk other) => OrderNumber == other.OrderNumber ? 0 : OrderNumber > other.OrderNumber ? 1 : -1;

        public int CompareTo(object obj) => CompareTo((IChunk)obj);

    }
}
