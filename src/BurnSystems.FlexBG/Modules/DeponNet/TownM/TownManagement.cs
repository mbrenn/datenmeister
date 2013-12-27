using BurnSystems.FlexBG.Modules.DeponNet.Common;
using BurnSystems.FlexBG.Modules.DeponNet.TownM.Interface;
using BurnSystems.FlexBG.Modules.IdGeneratorM;
using BurnSystems.ObjectActivation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.DeponNet.TownM
{
    /// <summary>
    /// Implements the town management
    /// </summary>
    public class TownManagement : ITownManagement
    {
        [Inject(IsMandatory = true)]
        public LocalTownDatabase Data
        {
            get;
            set;
        }

        [Inject(IsMandatory = true)]
        public IIdGenerator IdGenerator
        {
            get;
            set;
        }

        /// <summary>
        /// Creates a new town into the database
        /// </summary>
        /// <param name="userId">Id of the user</param>
        /// <param name="gameId">Id of the game</param>
        /// <param name="playerName">Name of the player</param>
        /// <param name="empireName">Name of the empire</param>
        /// <returns>Id of the new player</returns>
        public long CreateTown(long playerId, string townName, bool isCapital = false, ObjectPosition position = null)
        {
            var town = new Town()
            {
                Id = this.IdGenerator.NextId(EntityType.Town),
                IsCapital = isCapital,
                OwnerId = playerId,
                TownName = townName,
                Position = position
            };

            lock (this.Data.SyncObject)
            {
                this.Data.TownsStore.Towns.Add(town);
            }

            return town.Id;
        }

        /// <summary>
        /// Gets all towns of the player
        /// </summary>
        /// <param name="playerId">Id of player</param>
        /// <returns>Enumeration of all towns</returns>
        public IEnumerable<Town> GetTownsOfPlayer(long playerId)
        {
            lock (this.Data.SyncObject)
            {
                return this.Data.TownsStore.Towns.Where(x => x.OwnerId == playerId).ToList();
            }
        }

        /// <summary>
        /// Gets the town by specific town id
        /// </summary>
        /// <param name="townId">Id of the town</param>
        /// <returns>Retrieved town</returns>
        public Town GetTown(long townId)
        {
            lock (this.Data.SyncObject)
            {
                return this.Data.TownsStore.Towns.Where(x => x.Id == townId).FirstOrDefault();
            }
        }
    }
}
