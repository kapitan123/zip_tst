using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using Zipper.Chunk;

namespace Zipper.ConcurrentCollections
{
    public class BlockingPriorityQueue : IProducerConsumerCollection<IChunk>
    {
        private SortedChunksQueue _orderedList;

        private int _count = 0;
        public int Count => _count;

        public object SyncRoot => _orderedList.SyncRoot;

        public bool IsSynchronized => _orderedList.IsSynchronized;

        public BlockingPriorityQueue()
        {
            _orderedList = new SortedChunksQueue(true);
        }

        public bool TryAdd(IChunk chunk)
        {
            _orderedList.Add(chunk);

            Interlocked.Increment(ref _count);
            return true;
        }

        public bool TryTake(out IChunk item)
        {
            lock (_orderedList)
            {
                if (Count > 0)
                {
                    item = _orderedList.TakeFirst();
                    Interlocked.Decrement(ref _count);
                    return true;
                }
            }

            item = ChunkFactory.GetNull();
            return false;
        }

        public void CopyTo(IChunk[] destination, int destStartingIndex)
        {
            if (destination == null) throw new ArgumentNullException();
            if (destStartingIndex < 0) throw new ArgumentOutOfRangeException();

            var remaining = destination.Length;
            var temp = ToArray();
            for (int i = 0; i < destination.Length && i < temp.Length; i++)
            {
                destination[i] = temp[i];
            }
        }

        public IChunk[] ToArray()
        {
            lock (_orderedList)
            {
                return _orderedList.ToArray();
            }
        }

        public IEnumerator<IChunk> GetEnumerator()
        {
            foreach (var item in _orderedList)
            {
                yield return item;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        void ICollection.CopyTo(Array array, int index) => CopyTo(array as IChunk[], index);
    }
}
