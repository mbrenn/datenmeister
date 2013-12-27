using BurnSystems.FlexBG.Modules.DeponNet.BuildingM.Interface;
using BurnSystems.ObjectActivation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.DeponNet.MapFieldOwnerM
{
    public class FieldOwnershipByBuilding : MapFieldOwnerAlgorithm
    {
        [Inject]
        public IBuildingDataProvider BuildingDataProvider
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the configuration
        /// </summary>
        public new FieldOwnershipByBuildingConfig Config
        {
            get
            {
                return base.Config as FieldOwnershipByBuildingConfig;
            }
            set
            {
                base.Config = value;
            }
        }

        /// <summary>
        /// Gets all field owners
        /// </summary>
        /// <returns>Enumeration of field owners</returns>
        public override IEnumerable<FieldOwner> GetFieldOwners()
        {
            var buildings = this.BuildingDataProvider.GetAllBuildings();
            foreach (var building in buildings)
            {
                var intensity = this.Config.GetIntensityFor(building.BuildingTypeId);
                if (intensity == 0)
                {
                    continue;
                }

                yield return new FieldOwner()
                {
                    Position = building.Position,
                    OwnerId = building.PlayerId,
                    Influence = intensity
                };
            }
        }
    }
}
