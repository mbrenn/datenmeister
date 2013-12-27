using BurnSystems.FlexBG.Modules.DeponNet.BuildingM.Interface;
using BurnSystems.FlexBG.Modules.DeponNet.Common;
using BurnSystems.FlexBG.Modules.DeponNet.GameM;
using BurnSystems.FlexBG.Modules.DeponNet.GameM.Controllers;
using BurnSystems.FlexBG.Modules.DeponNet.TownM.Interface;
using BurnSystems.FlexBG.Modules.IdGeneratorM;
using BurnSystems.ObjectActivation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.DeponNet.BuildingM
{
    public class BuildingManagement : IBuildingManagement
    {
        /// <summary>
        /// Gets or sets the game database
        /// </summary>
        [Inject(IsMandatory = true)]
        public LocalBuildingDatabase BuildingDb
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the id generator
        /// </summary>
        [Inject()]
        public IIdGenerator IdGenerator
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the id generator
        /// </summary>
        [Inject()]
        public ITownManagement TownManagement
        {
            get;
            set;
        }

        /// <summary>
        /// Creates a building on a specific coordinate
        /// </summary>
        /// <param name="x">X-Coordinate of the building</param>
        /// <param name="y">Y-Coordinate of the building</param>
        /// <param name="buildingType">Type of the building</param>
        /// <param name="townId">Id of the town, where building shall be created</param>
        /// <returns>Id of the created building</returns>
        public long CreateBuilding(BuildingType buildingType, long townId, ObjectPosition position)
        {
            var ownerTown = this.TownManagement.GetTown(townId);
            if (ownerTown == null)
            {
                throw new InvalidOperationException("Town with id " + townId + " does not exist");
            }

            var building = new Building();
            building.Id = this.IdGenerator.NextId(EntityType.Building);
            building.IsActive = true;
            building.Level = 1;
            building.PlayerId = ownerTown.OwnerId;
            building.Productivity = 1;
            building.TownId = townId;
            building.BuildingTypeId = buildingType.Id;
            building.Position = position;

            lock (this.BuildingDb.BuildingsStore)
            {
                this.BuildingDb.BuildingsStore.Buildings.Add(building);
            }

            return building.Id;
        }

        /// <summary>
        /// Gets all buildings of the player
        /// </summary>
        /// <param name="playerId">Id of the player</param>
        /// <returns>Enumeration of buildings</returns>
        public IEnumerable<Building> GetBuildingsOfPlayer(long playerId)
        {
            lock (this.BuildingDb.BuildingsStore)
            {
                return this.BuildingDb.BuildingsStore.Buildings.Where(x => x.PlayerId == playerId).ToList();
            }
        }

        /// <summary>
        /// Gets all buildings of the town
        /// </summary>
        /// <param name="townId">Id of the town</param>
        /// <returns>Enumeration of towns</returns>
        public IEnumerable<Building> GetBuildingsOfTown(long townId)
        {
            lock (this.BuildingDb.BuildingsStore)
            {
                return this.BuildingDb.BuildingsStore.Buildings.Where(x => x.TownId == townId).ToList();
            }
        }
    }
}
