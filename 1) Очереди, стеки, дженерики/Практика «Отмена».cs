using System;
using System.Collections.Generic;

namespace LimitedSizeStack
{
    public class ListModel<TItem>
    {
        public List<TItem> Items { get; }
        private LimitedSizeStack<UndoItem<TItem>> undoStack;

        private class UndoItem<T>
        {
            public T Item { get; }
            public int Index { get; }
            public OperationType Type { get; }

            public UndoItem(T item, int index, OperationType type)
            {
                Item = item;
                Index = index;
                Type = type;
            }
        }

        private enum OperationType
        {
            Add,
            Remove
        }

        public ListModel(int undoLimit) : this(new List<TItem>(), undoLimit)
        {
        }

        public ListModel(List<TItem> items, int undoLimit)
        {
            Items = items;
            undoStack = new LimitedSizeStack<UndoItem<TItem>>(undoLimit);
        }

        public void AddItem(TItem item)
        {
            var undoItem = new UndoItem<TItem>(item, Items.Count, OperationType.Add);
            undoStack.Push(undoItem);
            Items.Add(item);
        }

        public void RemoveItem(int index)
        {
            var item = Items[index];
            var undoItem = new UndoItem<TItem>(item, index, OperationType.Remove);
            undoStack.Push(undoItem);
            Items.RemoveAt(index);
        }

        public bool CanUndo()
        {
            return undoStack.Count > 0;
        }

        public void Undo()
        {
            var undoItem = undoStack.Pop();
            switch (undoItem.Type)
            {
                case OperationType.Add:
                    Items.RemoveAt(undoItem.Index);
                    break;
                case OperationType.Remove:
                    Items.Insert(undoItem.Index, undoItem.Item);
                    break;
            }
        }
    }
}
