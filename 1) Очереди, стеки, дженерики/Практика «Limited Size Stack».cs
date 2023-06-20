using System;
using System.Collections.Generic;

namespace LimitedSizeStack
{
    public class LimitedSizeStack<T>
    {
        private readonly LinkedList<T> _list = new LinkedList<T>();
        private readonly int _stackSize;

        public LimitedSizeStack(int stackSize)
        {
            _stackSize = stackSize;
        }

        public void Push(T item)
        {
            _list.AddLast(item);
            if (_list.Count > _stackSize)
            {
                _list.RemoveFirst();
            }
        }

        public T Pop()
        {
            if (_list.Count == 0)
            {
                throw new InvalidOperationException("The stack is empty.");
            }
            T result = _list.Last.Value;
            _list.RemoveLast();
            return result;
        }

        public int Count => _list.Count;
    }
}
