using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.DeponNet.MapFieldOwnerM
{
    /// <summary>
    /// Defines the configuration for the importance of buildings
    /// </summary>
    public class FieldOwnershipByBuildingConfig : AlgorithmConfig
    {
        /// <summary>
        /// Stores the building information
        /// </summary>
        private Dictionary<long, double> buildingInformation = new Dictionary<long, double>();

        public void AddBuilding(long buildingTypeId, double intensity)
        {
            this.buildingInformation[buildingTypeId] = intensity;
        }

        /// <summary>
        /// Gets the intensity for a specific building type
        /// </summary>
        /// <param name="buildingTypeId">Id of the building type</param>
        /// <returns>Intensity for the building</returns>
        public double GetIntensityFor(long buildingTypeId)
        {
            double result;
            if (this.buildingInformation.TryGetValue(buildingTypeId, out result))
            {
                return result;
            }
            else
            {
                return 0.0;
            }
        }
    }
}
