using BurnSystems.FlexBG.Modules.DeponNet.BuildingM.Interface;
using BurnSystems.FlexBG.Modules.DeponNet.Common;
using BurnSystems.FlexBG.Modules.DeponNet.PlayerM;
using BurnSystems.FlexBG.Modules.DeponNet.PlayerM.Interface;
using BurnSystems.FlexBG.Modules.DeponNet.ResourceSetM;
using BurnSystems.FlexBG.Modules.DeponNet.ResourceSetM.Interface;
using BurnSystems.FlexBG.Modules.DeponNet.TownM.Interface;
using BurnSystems.FlexBG.Modules.DeponNet.UnitM.Data;
using BurnSystems.FlexBG.Modules.DeponNet.UnitM.Interfaces;
using BurnSystems.FlexBG.Modules.LockMasterM;
using BurnSystems.FlexBG.Modules.MapVoxelStorageM.Storage;
using BurnSystems.ObjectActivation;
using BurnSystems.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.DeponNet.Rules.PlayerRulesM
{
    /// <summary>
    /// Stores the rules, which are for the player
    /// </summary>
    public class PlayerRulesLogic : IPlayerRulesLogic
    {
        [Inject(IsMandatory=true)]
        public ILockMaster LockMaster
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the playermanagement
        /// </summary>
        [Inject(IsMandatory = true)]
        public IVoxelMap VoxelMap
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the playermanagement
        /// </summary>
        [Inject(IsMandatory=true)]
        public IPlayerManagement PlayerManagement
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the playermanagement
        /// </summary>
        [Inject(IsMandatory = true)]
        public ITownManagement TownManagement
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the playermanagement
        /// </summary>
        [Inject(IsMandatory = true)]
        public IBuildingManagement BuildingManagement
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the resourcemanagement
        /// </summary>
        [Inject(IsMandatory = true)]
        public IResourceManagement ResourceManagement
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the unit management
        /// </summary>
        [Inject(IsMandatory = true)]
        public IUnitManagement UnitManagement
        {
            get;
            set;
        }

        [Inject(IsMandatory = true)]
        public PlayerRulesConfig Config
        {
            get;
            set;
        }

        /// <summary>
        /// Creates a new player
        /// </summary>
        /// <param name="param">Parameters for the new player</param>
        /// <returns>Id of the new player</returns>
        public long CreatePlayer(PlayerCreationParams param)
        {
            using (this.LockMaster.AcquireWriteLock(EntityType.Game, param.GameId))
            {
                var playerId = this.PlayerManagement.CreatePlayer(
                    param.UserId,
                    param.GameId,
                    param.Playername,
                    param.Empirename);

                var position = this.FindStartRandomPositionForPlayer(param.GameId);

                var townId = this.TownManagement.CreateTown(
                    playerId,
                    param.FirstTownName,
                    true,
                    position);

                // Creates the building
                var buildingId = this.BuildingManagement.CreateBuilding(
                    GameConfig.Buildings.Temple,
                    townId,
                    position);

                // Initializes the player
                var playerResources = new ResourceSet();
                playerResources.Set(this.Config.PlayerStartResources);
                this.ResourceManagement.SetAvailable(EntityType.Player, playerId, playerResources);

                var townResources = new ResourceSet();
                townResources.Set(this.Config.TownStartResources);
                this.ResourceManagement.SetAvailable(EntityType.Town, townId, townResources);

                // Creates one constructor
                this.UnitManagement.CreateUnit(
                    playerId,
                    GameConfig.Units.Constructor.Id,
                    1,
                    new ObjectPosition(position.X + 1, position.Y + 1, 0));

                return playerId;
            }
        }

        /// <summary>
        /// Drops the player
        /// </summary>
        /// <param name="playerId">Id of the player to be dropped</param>
        public void DropPlayer(long playerId)
        {
            var player = this.PlayerManagement.GetPlayer(playerId);
            using (this.LockMaster.AcquireWriteLock(EntityType.Game, player.GameId))
            {
                this.PlayerManagement.RemovePlayer(playerId);
            }
        }

        public ObjectPosition FindStartRandomPositionForPlayer(long gameId)
        {
            var info = this.VoxelMap.GetInfo(gameId);
            Ensure.That(info != null, "No Information of map has been found");
            return new ObjectPosition(
                Math.Floor(MathHelper.Random.NextDouble() * info.SizeX), 
                Math.Floor(MathHelper.Random.NextDouble() * info.SizeY),
                0);
        }

        /// <summary>
        /// Gets a value whether the user may join the game
        /// </summary>
        /// <param name="userId">Id of the user</param>
        /// <param name="gameId">Id of the game</param>
        /// <returns>true, if user can join the game</returns>
        public bool CanUserContinueGame(long userId, long gameId)
        {
            return this.PlayerManagement.GetPlayersOfUser(userId).Any(x => x.GameId == gameId);
        }
    }
}
