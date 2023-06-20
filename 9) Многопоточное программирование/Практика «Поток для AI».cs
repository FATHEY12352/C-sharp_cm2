using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace rocket_bot
{
    public class Channel<T> where T : class
    {
        private List<T> items = new List<T>();

        public T this[int index]
        {
            get
            {
                lock (items)
                {
                    return index < items.Count ? items[index] : null;
                }
            }
            set
            {
                lock (items)
                {
                    if (index == items.Count)
                    {
                        items.Add(value);
                    }
                    else if (index < items.Count)
                    {
                        items.RemoveRange(index, items.Count - index);
                        items.Insert(index, value);
                    }
                }
            }
        }

        public T LastItem()
        {
            lock (items)
            {
                return items.Count > 0 ? items[^1] : null;
            }
        }

        public void AppendIfLastItemIsUnchanged(T item, T knownLastItem)
        {
            lock (items)
            {
                if (items.Count == 0 || LastItem() == knownLastItem)
                {
                    items.Add(item);
                }
            }
        }

        public int Count => items.Count;
    }
}
