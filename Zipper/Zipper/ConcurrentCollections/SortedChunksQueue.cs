using System.Collections;
using System.Collections.Generic;
using Zipper.Chunk;

namespace Zipper.ConcurrentCollections
{
    public class SortedChunksQueue : IEnumerable<IChunk>
    {
        private SortedList _queue = new SortedList();

        public bool IsSynchronized { get; }

        public object SyncRoot => _queue.SyncRoot;

        public SortedChunksQueue(bool synchronized)
        {
            IsSynchronized = synchronized;
            _queue = synchronized ? SortedList.Synchronized(new SortedList()) : new SortedList();
        }

        public void Add(IChunk chunk) => _queue.Add(chunk.OrderNumber, chunk);

        public IChunk TakeFirst()
        {
            var item = PeekFirst();
            RemoveFirst();
            return item;
        }       

        public IChunk GetFirstOrDefault() => _queue.Count == 0 ? ChunkFactory.GetNull() : PeekFirst();

        public IChunk PeekFirst() => (IChunk)_queue.GetByIndex(0);

        public void RemoveFirst() => _queue.RemoveAt(0);

        public IChunk[] ToArray()
        {
            var result = new IChunk[_queue.Count];
            int index = 0;
            foreach (var q in _queue)
            {
                result[index] = (IChunk)q;
                index++;
            }

            return result;
        }

        public IEnumerator<IChunk> GetEnumerator()
        {
            foreach (var item in _queue)
            {
                yield return (IChunk)item;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
