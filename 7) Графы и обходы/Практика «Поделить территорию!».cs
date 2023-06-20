using System.Collections.Generic;
using System.Drawing;

namespace Rivals
{
    public class RivalsTask
    {
        public static IEnumerable<OwnedLocation> AssignOwners(Map map)
        {
            var queue = new Queue<OwnedLocation>();
            var visitedPoints = new HashSet<Point>();

            for (int n = 0; n < map.Players.Length; n++)
            {
                var player = map.Players[n];
                queue.Enqueue(new OwnedLocation(n, player, 0));
                visitedPoints.Add(player);
            }

            while (queue.Count > 0)
            {
                var ownedLocation = queue.Dequeue();
                var currLocationPoint = ownedLocation.Location;

                yield return new OwnedLocation(ownedLocation.Owner, 
                    currLocationPoint, ownedLocation.Distance);

                EnqueueNeighboringPoints(queue, ownedLocation, 
                    currLocationPoint, map, visitedPoints);
            }
        }

        private static void EnqueueNeighboringPoints(Queue<OwnedLocation>
            queue, OwnedLocation ownedLocation, Point currLocationPoint, 
            Map map, HashSet<Point> visitedPoints)
        {
            var neighbors = GetNeighborPoints(currLocationPoint);

            foreach (var neighbor in neighbors)
            {
                if (CanSkipPoint(map, neighbor, visitedPoints))
                {
                    continue;
                }

                queue.Enqueue(new OwnedLocation(ownedLocation.Owner, 
                    neighbor, ownedLocation.Distance + 1));
                visitedPoints.Add(neighbor);
            }
        }

        private static IEnumerable<Point> GetNeighborPoints(Point point)
        {
            yield return new Point(point.X - 1, point.Y);
            yield return new Point(point.X + 1, point.Y);
            yield return new Point(point.X, point.Y - 1);
            yield return new Point(point.X, point.Y + 1);
        }

        private static bool CanSkipPoint(Map map, Point point,
            HashSet<Point> visitedPoints)
        {
            return !map.InBounds(point) || visitedPoints.Contains(point) 
                || map.Maze[point.X, point.Y] != MapCell.Empty;
        }
    }
}
