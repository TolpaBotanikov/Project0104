using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public class AStarSearch
    {
        public Dictionary<Cell, Cell> cameFrom
        = new Dictionary<Cell, Cell>();
        public Dictionary<Cell, double> costSoFar
            = new Dictionary<Cell, double>();
        public Cell _start;
        public Cell _goal;

        public AStarSearch(Battlefield graph, Cell start, Cell goal)
        {
            _start = start;
            _goal = goal;
            var frontier = new PriorityQueue<Cell>();
            frontier.Enqueue(start, 0);

            cameFrom[start] = start;
            costSoFar[start] = 0;

            while (frontier.Count > 0)
            {
                var current = frontier.Dequeue();

                if (current.Equals(goal))
                {
                    break;
                }

                foreach (var next in graph.Neighbors(current))
                {
                    double newCost = costSoFar[current]
                        + 1;
                    if (!costSoFar.ContainsKey(next)
                        || newCost < costSoFar[next])
                    {
                        costSoFar[next] = newCost;
                        double priority = newCost + Cell.Distance(next.position, goal.position);
                        frontier.Enqueue(next, priority);
                        cameFrom[next] = current;
                    }
                }
            }
        }
    }
}
