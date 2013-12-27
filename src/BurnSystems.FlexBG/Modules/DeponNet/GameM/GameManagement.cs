using BurnSystems.FlexBG.Modules.DeponNet.GameM.Interface;
using BurnSystems.FlexBG.Modules.IdGeneratorM;
using BurnSystems.ObjectActivation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.DeponNet.GameM
{
    /// <summary>
    /// Defines the game managemnet
    /// </summary>
    public class GameManagement : IGameManagement
    {
        /// <summary>
        /// Gets or sets the game database
        /// </summary>
        [Inject(IsMandatory = true)]
        public LocalGameDatabase GameDb
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
        /// Gets the games
        /// </summary>
        /// <returns>Games to be included</returns>
        public IEnumerable<Game> GetAll()
        {
            lock (this.GameDb.GameStore)
            {
                return this.GameDb.GameStore.Games.ToList();
            }
        }

        /// <summary>
        /// Creates the game
        /// </summary>
        /// <param name="title">Title of the game</param>
        /// <param name="description">Description of the game</param>
        /// <param name="maxPlayers">Allowed player count</param>
        /// <returns>Id of the created game</returns>
        public long Create(string title, string description, int maxPlayers)
        {
            var nextGameId = this.IdGenerator.NextId(EntityType.Game);
            var game = new Game()
            {
                Id = nextGameId,
                Title = title,
                Description = description,
                MaxPlayers = maxPlayers,
                IsPaused = true
            };

            lock (this.GameDb.GameStore)
            {
                this.GameDb.GameStore.Games.Add(game);
            }

            return nextGameId;
        }

        /// <summary>
        /// Gets a game by id
        /// </summary>
        /// <param name="gameId">Id of the game</param>
        /// <returns>Game which has been found</returns>
        public Game Get(long gameId)
        {
            lock (this.GameDb.GameStore)
            {
                return this.GameDb.GameStore.Games.Where(x => x.Id == gameId).FirstOrDefault();
            }
        }
    }
}
