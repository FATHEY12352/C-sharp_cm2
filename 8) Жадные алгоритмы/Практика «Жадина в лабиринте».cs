using System;
using System.Collections.Generic;
using System.Linq;
using Greedy.Architecture;

namespace Greedy
{
    public class GreedyPathFinder : IPathFinder
    {
        public List<Point> FindPathToCompleteGoal(State state)
        {
            List<Point> resList = new List<Point>();
            int energy = 0;
            Point startPosition = state.Position;
            HashSet<Point> chestsPoints = state.Chests.ToHashSet();

            for (int n = 0; n < state.Goal; n++)
            {
                PathWithCost pathCost = GetShortestPath(state, startPosition, chestsPoints);

                if (pathCost != null)
                {
                    energy += pathCost.Cost;

                    if (energy <= state.Energy)
                    {
                        chestsPoints.Remove(pathCost.End);
                        List<Point> preList = GetPathExcludingStartPoint(pathCost.Path);
                        resList.AddRange(preList);
                        startPosition = pathCost.End;
                    }
                    else
                    {
                        return new List<Point>();
                    }
                }
                else
                {
                    return new List<Point>();
                }
            }
            return resList;
        }

        private PathWithCost GetShortestPath(State state,
            Point startPosition, HashSet<Point> chestsPoints)
        {
            DijkstraPathFinder dijkstraPathFinder = new DijkstraPathFinder();
            return dijkstraPathFinder.GetPathsByDijkstra 
                (state, startPosition, chestsPoints).FirstOrDefault();
        }

        private List<Point> GetPathExcludingStartPoint(List<Point> path)
        {
            return path.Skip(1).ToList();
        }
    }
}
