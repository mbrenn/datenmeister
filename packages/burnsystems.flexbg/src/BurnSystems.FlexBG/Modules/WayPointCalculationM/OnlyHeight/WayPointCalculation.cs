using BurnSystems.Collections;
using BurnSystems.FlexBG.Modules.DeponNet.Common;
using BurnSystems.FlexBG.Modules.DeponNet.GameM;
using BurnSystems.FlexBG.Modules.DeponNet.UnitM;
using BurnSystems.FlexBG.Modules.MapVoxelStorageM.Storage;
using BurnSystems.Logging;
using BurnSystems.ObjectActivation;
using BurnSystems.Test;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.WayPointCalculationM.OnlyHeight
{
    /// <summary>
    /// Calculates the way points just by considering the heights. 
    /// It does not consider the blockage by buildings or other objects.
    
    /// This class cannot be used by multiple threads simultaneously. 
    /// The waypoint calculation uses the A* algorithm derived from wikipedia
    /// </summary>
    public class WayPointCalculation : IWayPointCalculation
    {
        /// <summary>
        /// Stores the class logger
        /// </summary>
        private static ILog logger = new ClassLogger(typeof(WayPointCalculation));
        [Inject]
        public IVoxelMap VoxelMap
        {
            get;
            set;
        }

        [Inject(ByName="CurrentGame", IsMandatory=true)]
        public Game Game
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether a full debug shall be executed with the help of the
        /// integrated class logger
        /// </summary>
        public bool DoFullDebug
        {
            get;
            set;
        }

        private HashSet<NodeInfo> closedSet;

        private PriorityQueue<NodeInfo> openSet;

        private Dictionary<NodeInfo, NodeInfo> cameFrom;

        /// <summary>
        /// Stores the nodes which can be retrieved if necessary
        /// </summary>
        private Dictionary<Pair<int, int>, NodeInfo> nodeCache;

        /// <summary>
        /// Influence of the height
        /// </summary>
        private double heightBase = 1.1;

        /// <summary>
        /// Influence of the height
        /// </summary>
        private double heightFactor = 0.5;

        /// <summary>
        /// Constant value
        /// </summary>
        private double constValue = 0.5;

        /// <summary>
        /// Stores the endnode
        /// </summary>
        private NodeInfo endNode;

        /// <summary>
        /// Maximum X-Position
        /// </summary>
        private long maxX;

        /// <summary>
        /// Maximum Y-Position
        /// </summary>
        private long maxY;

        /// <summary>
        /// Performs the calculation
        /// </summary>
        /// <param name="startPosition">Start Position</param>
        /// <param name="endPosition">End Position</param>
        /// <param name="unitType">Type of the unit</param>
        /// <returns>Calculated way points</returns>
        public IEnumerable<ObjectPosition> CalculateWaypoints(ObjectPosition startPosition, ObjectPosition endPosition, UnitType unitType)
        {
            logger.LogEntry(
                   LogLevel.Notify,
                   string.Format(
                       "Executing waypoint calculation from {0} to {1}",
                       startPosition.ToString(),
                       endPosition.ToString()));

            var watch = new Stopwatch();
            watch.Start();

            this.InitAStar();

            int startX, startY, endX, endY;
            this.AlignToFieldCoordinates(startPosition, out startX, out startY);
            this.AlignToFieldCoordinates(endPosition, out endX, out endY);

            var startNode = this.CreateNewNode(startX, startY);
            this.endNode = this.CreateNewNode(endX, endY);

            startNode.HeuristicCost = this.GetHeuristicCost(startNode, endNode);

            this.openSet.Add(startNode);

            // Performs the algorithm
            var result = this.PerformAStar();
            watch.Stop();

            int stepCount = 0;
            if (result != null)
            {
                result = result.ToList();
                stepCount = result.Count();
            }

            logger.LogEntry(
                LogLevel.Notify,
                string.Format(
                    "Executed waypoint calculation, having {3} steps from {0} to {1} within {2} ms ({4} nodes in closed list, {5} nodes open)",
                    startPosition.ToString(),
                    endPosition.ToString(),
                    watch.ElapsedMilliseconds.ToString(),
                    stepCount,
                    this.closedSet.Count,
                    this.openSet.Count));

            if (result == null)
            {
                return null;
            }

            // Returns the list of positions
            // Addition of 0.5 is necessary because central positions will be returned. 
            return result.Select(x =>
                    new ObjectPosition(x.X + 0.5, x.Y + 0.5, 0));
        }

        /// <summary>
        /// Performs the initialization for A* 
        /// </summary>
        private void InitAStar()
        {
            this.openSet = new PriorityQueue<NodeInfo>((x, y) => y.HeuristicCost.CompareTo(x.HeuristicCost));
            this.closedSet = new HashSet<NodeInfo>();
            this.cameFrom = new Dictionary<NodeInfo, NodeInfo>();
            this.nodeCache = new Dictionary<Pair<int, int>, NodeInfo>();

            var info = this.VoxelMap.GetInfo(this.Game.Id);
            this.maxX = info.SizeX;
            this.maxY = info.SizeY;
        }

        /// <summary>
        /// Creates a new node or returns a cached one if necessary
        /// </summary>
        /// <param name="x">X-Coordinate of the node</param>
        /// <param name="y">Y-Coordinate of the node</param>
        /// <returns>Node (created or cached)</returns>
        private NodeInfo CreateNewNode(int x, int y)
        {
            NodeInfo result;
            if (this.nodeCache.TryGetValue(new Pair<int, int>(x, y), out result))
            {
                return result;
            }
            
            result = new NodeInfo(x, y);

            return result;
        }

        /// <summary>
        /// Performs the A* algorithm
        /// </summary>
        private IEnumerable<NodeInfo> PerformAStar()
        {
            while (this.openSet.Count > 0)
            {
                var current = this.openSet.Pop();
                if (current == null)
                {
                    return null;
                }

                if (this.DoFullDebug)
                {
                    logger.LogEntry(LogLevel.Verbose, "Popped waypoint: " + current.ToString());
                }

                if (current.X == endNode.X && current.Y == endNode.Y)
                {
                    // Reconstruct Path
                    return this.ReconstructPath(current);
                }

                this.closedSet.Add(current);

                // Go through all neighbors
                foreach (var neighbor in this.GetNeighbors(current))
                {
                    if (this.closedSet.Contains(neighbor))
                    {
                        // Already captured
                        continue;
                    }

                    // Calculate score:
                    var effort = this.GetEffort(current, neighbor);
                    if (effort == Double.MaxValue)
                    {
                        // Not interesting, do not put in open list
                        continue;
                    }

                    var tempCost = current.CalculatedCost + this.GetEffort(current, neighbor);
                    var isNeighborInOpenSet = this.openSet.Any(x => x.EqualsTo(neighbor));
                    if (!isNeighborInOpenSet
                        || tempCost <= neighbor.CalculatedCost)
                    {
                        this.cameFrom[neighbor] = current;
                        neighbor.CalculatedCost = tempCost;
                        neighbor.HeuristicCost = tempCost + this.GetHeuristicCost(neighbor, this.endNode);

                        if (!isNeighborInOpenSet)
                        {
                            this.openSet.Add(neighbor);

                            if (this.DoFullDebug)
                            {
                                logger.LogEntry(LogLevel.Verbose, "Pushed waypoint: " + neighbor.ToString());
                            }
                        }
                    }
                }
            }

            // No path
            return null;
        }

        /// <summary>
        /// Gets all neighbors 
        /// </summary>
        /// <param name="current">Current node</param>
        /// <returns>Enumeration of neighbors</returns>
        private IEnumerable<NodeInfo> GetNeighbors(NodeInfo current)
        {
            yield return this.CreateNewNode(current.X, current.Y - 1);
            yield return this.CreateNewNode(current.X, current.Y + 1);
            yield return this.CreateNewNode(current.X - 1, current.Y);
            yield return this.CreateNewNode(current.X + 1, current.Y);
        }

        /// <summary>
        /// Performs the reconstruction of path
        /// </summary>
        /// <param name="endNode">Current node to be used</param>
        /// <returns>Enumeration of backnodes</returns>
        private IEnumerable<NodeInfo> ReconstructPath(NodeInfo endNode)
        {
            var current = endNode;
            var result = new Stack<NodeInfo>();

            while (true)
            {
                result.Push(current);

                if (!this.cameFrom.TryGetValue(current, out current))
                {
                    break;
                }
            }

            while (result.Count > 0)
            {
                yield return result.Pop();
            }
        }

        /// <summary>
        /// Aligns the vector to 
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        private void AlignToFieldCoordinates(ObjectPosition position, out int x, out int y)
        {
            x = (int)Math.Floor(position.X);
            y = (int)Math.Floor(position.Y);
        }

        private double GetHeight(NodeInfo info)
        {
            if (!double.IsNaN(info.CachedHeight))
            {
                return info.CachedHeight;
            }

            info.CachedHeight = this.GetHeight(info.X, info.Y);
            return info.CachedHeight;
        }

        /// <summary>
        /// Gets the height of the field for the column
        /// </summary>
        /// <param name="x">X-Coordinate of the column</param>
        /// <param name="y">Y-Coordinate of the column</param>
        /// <returns>Height of the column</returns>
        private double GetHeight(int x, int y)
        {
            if (x < 0 || x >= this.maxX || y < 0 || y >= this.maxY)
            {
                return double.MaxValue;
            }

            return this.VoxelMap.GetHeight(
                this.Game.Id,
                x,
                y);
        }


        /// <summary>
        /// Gets the effort for the given unit
        /// </summary>
        /// <param name="startNode">First Node</param>
        /// <param name="endNode">Second Node</param>
        /// <returns>Effort for the nodes</returns>
        private double GetEffort(NodeInfo startNode, NodeInfo endNode)
        {
            return this.GetEffort(
                this.GetHeight(startNode),
                this.GetHeight(endNode));
        }

        /// <summary>
        /// Gets the effort for the given unit
        /// </summary>
        /// <param name="heightSource">First height</param>
        /// <param name="heightTarget">Second height</param>
        /// <returns>Effort for the height</returns>
        private double GetEffort(double heightSource, double heightTarget)
        {
            // Boundary checks
            if (heightSource >= float.MaxValue)
            {
                return this.constValue;
            }

            if (heightTarget >= float.MaxValue)
            {
                return double.MaxValue;
            }

            // Calculation itself
            // e = f * b^(h1-h2) + c
            return
                this.heightFactor * Math.Pow(this.heightBase, (heightTarget - heightSource))
                + this.constValue;
        }

        /// <summary>
        /// Gets the heurestic cost for the nodes from start to end
        /// </summary>
        /// <param name="start">Starting node</param>
        /// <param name="end">Ending node</param>
        /// <returns>Heurestic cost</returns>
        private double GetHeuristicCost(NodeInfo start, NodeInfo end)
        {
            return this.constValue * (Math.Abs(start.X - end.X) + Math.Abs(start.Y - end.Y));
        }
    }
}
