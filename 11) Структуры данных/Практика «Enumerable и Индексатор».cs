using Microsoft.VisualBasic;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;

namespace BinaryTrees;

public class BinaryTree<T> : IEnumerable<T>, IEnumerable
    where T : IComparable
{
    public TreeNode<T> root;

    public void Add(T key)
    {
        if (root == null)
        {
            root = new TreeNode<T>(key);
        }
        else
        {
            AddNode(root, key);
        }
    }

    private void AddNode(TreeNode<T> node, T key)
    {
        if (key.CompareTo(node.Value) < 0)
        {
            if (node.Left != null)
            {
                AddNode(node.Left, key);
            }
            else
            {
                node.Left = new TreeNode<T>(key);
            }
        }
        else
        {
            if (node.Right != null)
            {
                AddNode(node.Right, key);
            }
            else
            {
                node.Right = new TreeNode<T>(key);
            }
        }
    }


    public bool Contains(T key)
    {
        var node = root;

        while (node != null)
        {
            var diff = node.Value.CompareTo(key);
            if (diff == 0) return true;
            node = diff > 0 ? node.Left : node.Right;
        }
        return false;
    }

    public IEnumerator<T> GetEnumerator()
    {
        if (root == null) yield break;
        foreach (var item in root?.GetValues())
        {
            yield return item;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public T this[int index]
    {
        get
        {
            if (root == null || index < 0 || index >= root.Size) throw new IndexOutOfRangeException();
            return FindNodeByIndex(root, index);
        }
    }
    private T FindNodeByIndex(TreeNode<T> node, int index)
    {
        var leftSize = node.Left?.Size ?? 0;
        if (index == leftSize)
            return node.Value;
        else if (index < leftSize)
            return FindNodeByIndex(node.Left, index);
        else
            return FindNodeByIndex(node.Right, index - leftSize - 1);
    }
}

public class TreeNode<T>
{
    public T Value { get; private set; }
    private TreeNode<T> left, right, parent;

    public int Size { get; private set; }

    public TreeNode(T value)
    {
        if (value == null)
            throw new ArgumentNullException(nameof(value));
        Value = value;
        Size = 1;
    }

    public TreeNode<T> Left
    {
        get { return left; }
        set
        {
            if (left != null) OnChildCountChanged(-left.Size);
            left = value;
            if (value != null)
            {
                OnChildCountChanged(value.Size);
                value.parent = this;
            }
        }
    }

    public TreeNode<T> Right
    {
        get { return right; }
        set
        {
            if (right != null) OnChildCountChanged(-right.Size);
            right = value;
            if (value != null)
            {
                OnChildCountChanged(value.Size);
                value.parent = this;
            }
        }
    }

    public IEnumerable<T> GetValues()
    {
        if (Left != null)
            foreach (var value in Left.GetValues())
                yield return value;
        yield return Value;
        if (Right != null)
            foreach (var value in Right.GetValues())
                yield return value;
    }

    private void OnChildCountChanged(int delta)
    {
        Size += delta;
        parent?.OnChildCountChanged(delta);
    }
}