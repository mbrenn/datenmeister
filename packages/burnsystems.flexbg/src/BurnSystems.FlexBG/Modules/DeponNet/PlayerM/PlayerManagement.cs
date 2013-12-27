using BurnSystems.FlexBG.Modules.DeponNet.PlayerM.Interface;
using BurnSystems.FlexBG.Modules.IdGeneratorM;
using BurnSystems.ObjectActivation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.DeponNet.PlayerM
{
    public class PlayerManagement : IPlayerManagement
    {
        [Inject(IsMandatory=true)]
        public LocalPlayerDatabase Data
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
        /// Creates a player by user and game
        /// </summary>
        /// <param name="userId">Id of the associated user</param>
        /// <param name="gameId">Id of the associated game</param>
        /// <param name="playerName">Name of the player</param>
        /// <param name="empireName">Name of the empire</param>
        /// <returns>Id of the new player</returns>
        public long CreatePlayer(long userId, long gameId, string playerName, string empireName)
        {
            var player = new Player();
            player.OwnerId = userId;
            player.Playername = playerName;
            player.Empirename = empireName;
            player.GameId = gameId;
            player.Id = this.IdGenerator.NextId(EntityType.Player);

            lock (this.Data.SyncObject)
            {
                this.Data.PlayersStore.Players.Add(player);
            }

            return player.Id;
        }

        public void RemovePlayer(long playerId)
        {
            lock (this.Data.SyncObject)
            {
                this.Data.PlayersStore.Players.RemoveAll(x => x.Id == playerId);
            }
        }

        public IEnumerable<Player> GetAllPlayers()
        {
            lock (this.Data.SyncObject)
            {
                return this.Data.PlayersStore.Players.ToList();
            }
        }

        public IEnumerable<Player> GetPlayersOfUser(long userId)
        {
            lock (this.Data.SyncObject)
            {
                return this.Data.PlayersStore.Players.Where(x => x.OwnerId == userId).ToList();
            }
        }

        public IEnumerable<Player> GetPlayersOfGame(long gameId)
        {
            lock (this.Data.SyncObject)
            {
                return this.Data.PlayersStore.Players.Where(x => x.GameId == gameId).ToList();
            }
        }

        public Player GetPlayer(long playerId)
        {
            lock (this.Data.SyncObject)
            {
                return this.Data.PlayersStore.Players.Where(x => x.Id == playerId).FirstOrDefault();
            }
        }
    }
}
