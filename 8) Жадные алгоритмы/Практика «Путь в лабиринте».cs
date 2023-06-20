using System;
using System.Collections.Generic;
using System.Linq;
using Greedy.Architecture;

namespace Greedy
{
    public class DijkstraPathFinder
    {
        public IEnumerable<PathWithCost> GetPathsByDijkstra(State state, Point start, IEnumerable<Point> targets)
        {
            var notVisited = new List<Point> { start };
            var track = new Dictionary<Point, DijkstraData>
            { [start] = new DijkstraData { Previous = new Point(-1, -1), Price = 0 } };

            while (notVisited.Count > 0)
            {
                var toOpen = SelectOpenPoint(track, notVisited);

                if (toOpen == new Point(-1, -1)) yield break;

                var neighbors = AddPointsAround(state, toOpen, notVisited, track);

                if (targets.Contains(toOpen)) yield return CollectResult(track, toOpen);

                OpenPoint(state, toOpen, track, neighbors);

                notVisited.Remove(toOpen);
            }
        }

        private static Point SelectOpenPoint(Dictionary<Point, DijkstraData> track, List<Point> notVisited)
        {
            var bestPrice = double.PositiveInfinity;
            var toOpen = new Point(-1, -1);

            foreach (var item in notVisited)
            {
                if (track.ContainsKey(item) && track[item].Price < bestPrice)
                {
                    bestPrice = track[item].Price;
                    toOpen = item;
                }
            }

            return toOpen;
        }

        private static void OpenPoint(State state, Point toOpen,
            Dictionary<Point, DijkstraData> track, List<Point> neighbors)
        {
            foreach (var item in neighbors)
            {
                var currentPrice = track[toOpen].Price + state.CellCost[item.X, item.Y];
                var nextNode = item;
                if (!track.ContainsKey(nextNode) || track[nextNode].Price > currentPrice)
                    track[nextNode] = new DijkstraData { Previous = toOpen, Price = currentPrice };
            }
        }

        private static PathWithCost CollectResult(Dictionary<Point, DijkstraData> track, Point end)
        {
            var result = new List<Point>();
            int cost = track[end].Price;

            while (end != new Point(-1, -1))
            {
                result.Add(end);
                end = track[end].Previous;
            }

            result.Reverse();
            return new PathWithCost(cost, result.ToArray());
        }

        private static List<Point> AddPointsAround(State state,
            Point toOpen, List<Point> notVisited, Dictionary<Point, DijkstraData> track)
        {
            var neighbors = new List<Point>();

            for (int dy = -1; dy <= 1; dy++)
            {
                for (int dx = -1; dx <= 1; dx++)
                {
                    if (dy != 0 && dx != 0) continue;

                    var point = new Point(dx + toOpen.X, dy + toOpen.Y);

                    if (state.InsideMap(point) && !state.IsWallAt(point) && !track.ContainsKey(point))
                    {
                        notVisited.Add(point);
                        neighbors.Add(point);
                    }
                }
            }

            return neighbors;
        }
    }

    public class DijkstraData
    {
        public Point Previous { get; set; }
        public int Price { get; set; }
    }
}
