using System;
using System.Collections.Generic;
using System.Linq;

namespace linq_slideviews
{
    public static class ExtensionsTask
    {
        public static double Median(this IEnumerable<double> items)
        {
            var sortedItems = items.OrderBy(x => x).ToList();
            int count = sortedItems.Count;
            if (count == 0) throw new InvalidOperationException("Sequence contains no elements");

            int middleIndex = count / 2;
            double median = count % 2 == 0 ? (sortedItems[middleIndex - 1] +  
                sortedItems[middleIndex]) / 2.0 : sortedItems[middleIndex];

            return median;
        }

        public static IEnumerable<(T First, T Second)> Bigrams<T>(this IEnumerable<T> items)
        {
            T prev = default;
            bool first = true;

            foreach (T item in items)
            {
                if (!first)
                {
                    yield return (prev, item);
                }

                prev = item;
                first = false;
            }
        }
    }
}
