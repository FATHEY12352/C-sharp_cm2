using NUnit.Framework;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using static DiskTree.DiskTreeTask;

namespace DiskTree
{
    public class DiskTreeTask
    {
        public class Root
        {
            public string Name;
            public Dictionary<string, Root> Nodes = new Dictionary<string, Root>();

            public Root(string name)
            {
                Name = name;
            }

            public Root GetDirection(string subRoot)
            {
                if (Nodes.TryGetValue(subRoot, out Root node))
                {
                    return node;
                }
                else
                {
                    Root newRoot = new Root(subRoot);
                    Nodes[subRoot] = newRoot;
                    return newRoot;
                }
            }

            public List<string> MakeConclusion(int i, List<string> list)
            {
                if (i >= 0)
                    list.Add(new string(' ', i) + Name);
                i++;

                foreach (var child in Nodes
                    .Values
                    .OrderBy(root => root.Name, StringComparer.Ordinal))
                    list = child.MakeConclusion(i, list);
                return list;
            }
        }

        public static List<string> Solve(List<string> input)
        {
            var root = new Root("");
            foreach (var name in input)
            {
                var path = name.Split('\\');
                var currentNode = root;
                foreach (var item in path)
                {
                    currentNode = currentNode.GetDirection(item);
                }
            }

            var conclusionList = new List<string>();
            root.MakeConclusion(-1, conclusionList);
            return conclusionList;
        }
    }
}