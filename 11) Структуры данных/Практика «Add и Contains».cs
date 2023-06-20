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
        root = AddRecursive(root, key);
    }

    private TreeNode<T> AddRecursive(TreeNode<T> node, T key)
    {
        if (node == null)
        {
            return new TreeNode<T> { Value = key };
        }

        if (key.CompareTo(node.Value) < 0)
        {
            node.Left = AddRecursive(node.Left, key);
        }
        else
        {
            node.Right = AddRecursive(node.Right, key);
        }

        return node;
    }

    public bool Contains(T key)
    {
        return ContainsRecursive(root, key);
    }

    private bool ContainsRecursive(TreeNode<T> node, T key)
    {
        if (node == null)
        {
            return false;
        }

        if (node.Value.Equals(key))
        {
            return true;
        }
        else if (key.CompareTo(node.Value) < 0)
        {
            return ContainsRecursive(node.Left, key);
        }
        else
        {
            return ContainsRecursive(node.Right, key);
        }
    }


    public IEnumerator<T> GetEnumerator()
    {
        if (root == null) throw new ArgumentException();
        TreeNode<T> node = root;
        return MakeEnumerate(node);
    }

    private IEnumerator<T> MakeEnumerate(TreeNode<T> node)
    {
        if (node.Left == null) yield return node.Value;
        else MakeEnumerate(node.Left);
        if (node.Right == null) yield break;
        else MakeEnumerate(node.Right);
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

public class TreeNode<T>
{
    public T Value;
    public TreeNode<T> Left, Right;
}