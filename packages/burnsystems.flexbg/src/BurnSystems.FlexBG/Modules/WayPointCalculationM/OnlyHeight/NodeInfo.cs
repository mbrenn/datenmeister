using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.WayPointCalculationM.OnlyHeight
{
    public class NodeInfo
    {
        /// <summary>
        /// X-Coordinate of the node
        /// </summary>
        public int X
        {
            get;
            set;
        }

        /// <summary>
        /// Y coordinate of the node
        /// </summary>
        public int Y
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the Calculated cost for the node. 
        /// Also called g_score in Wikipedia http://en.wikipedia.org/wiki/A-star
        /// </summary>
        public double CalculatedCost
        {
            get;
            set;
        }
        
        /// <summary>
        /// Gets or sets the Heuristic cost for the node. 
        /// Also called f_score in Wikipedia http://en.wikipedia.org/wiki/A-star
        /// </summary>
        public double HeuristicCost
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the cached height, speeding up the retrieval of height for open nodes
        /// </summary>
        public double CachedHeight
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes a new instance of the NodeInfo
        /// </summary>
        /// <param name="x">X-Coordinate of the node</param>
        /// <param name="y">Y-Coordinate of the node</param>
        public NodeInfo(int x, int y)
        {
            this.X = x;
            this.Y = y;
            this.CachedHeight = double.NaN;
        }

        /// <summary>
        /// Gets hashcode for dictionary
        /// </summary>
        /// <returns>Hashcode being used</returns>
        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode();
        }

        /// <summary>
        /// True, if equal
        /// </summary>
        /// <param name="obj">Other node</param>
        /// <returns>true, if equal</returns>
        public override bool Equals(object obj)
        {
            if (this == obj)
            {
                return true;
            }

            var objNode = obj as NodeInfo;
            if (objNode == null)
            {
                return false;
            }

            return objNode.X == this.X && objNode.Y == this.Y;
        }

        /// <summary>
        /// True, if equal
        /// </summary>
        /// <param name="objNode">Node to be checked</param>
        /// <returns>true, if equal</returns>
        internal bool EqualsTo(NodeInfo objNode)
        {
            return this == objNode || (objNode.X == this.X && objNode.Y == this.Y);
        }

        public override string ToString()
        {
            return string.Format(
                "{0}, {1} (Cost: {2}, Heuristic: {3}",
                this.X,
                this.Y,
                this.CalculatedCost,
                this.HeuristicCost);
        }
    }
}
