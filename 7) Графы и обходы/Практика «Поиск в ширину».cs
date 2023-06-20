using System.Collections.Generic;
using System.Linq;

namespace Dungeon
{
    public class BfsTask
    {
        public static IEnumerable<SinglyLinkedList<Point>> FindPaths(Map map, Point start, Point[] chests)
        {
            var queue = new Queue<SinglyLinkedList<Point>>();
            var visited = new HashSet<Point>();
            var myChest = new HashSet<Point>(chests);
            queue.Enqueue(new SinglyLinkedList<Point>(start, null));

            while (queue.Count > 0)
            {
                var point = queue.Dequeue();
                if (CheckPath(map, point.Value, visited))
                    continue;

                visited.Add(point.Value);
                if (myChest.Contains(point.Value))
                {
                    myChest.Remove(point.Value);
                    yield return point;
                }

                if (myChest.Count == 0)
                    yield break;

                var neighbors = GetNeighbors(point.Value);
                foreach (var neighbor in neighbors)
                {
                    queue.Enqueue(new SinglyLinkedList<Point>(neighbor, point));
                }
            }
        }

        private static bool CheckPath(Map map, Point point, HashSet<Point> visited)
        {
            return !map.InBounds(point) ||
                   visited.Contains(point) ||
                   map.Dungeon[point.X, point.Y] == MapCell.Wall;
        }

        private static IEnumerable<Point> GetNeighbors(Point point)
        {
            for (var dy = -1; dy <= 1; dy++)
            {
                for (var dx = -1; dx <= 1; dx++)
                {
                    if (dx == 0 && dy == 0)
                        continue;

                    if (dx != 0 && dy != 0)
                        continue;

                    var neighbor = new Point(point.X + dx, point.Y + dy);
                    yield return neighbor;
                }
            }
        }
    }
}
