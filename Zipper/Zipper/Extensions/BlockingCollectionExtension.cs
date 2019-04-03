using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Zipper.Extensions
{
    public static class BlockingCollectionExtension
    {
        public static void AddRange<T>(this BlockingCollection<T> collection, IEnumerable<T> colToAdd)
        {
            foreach (var item in colToAdd)
            {
                collection.Add(item);
            }
        }
    }
}
